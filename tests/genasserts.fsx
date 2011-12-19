open System
open System.Data
open System.IO
open System.Xml
open System.Globalization
open System.Text.RegularExpressions

let main() =
    let table = new DataTable()
    table.ReadXml(XmlReader.Create(Console.In)) |> ignore
    let cols = [ for col : DataColumn in table.Columns -> (col.ColumnName, col.DataType) ]
    let format fmt (v : obj) = String.Format(CultureInfo.InvariantCulture, fmt, [| v |])
    let csliteral = ""
    let rows = seq {
        for row : DataRow in table.Rows do 
            yield seq {
                for (name, typ), value in (row.ItemArray |> Seq.zip cols) do
                    let literal = 
                        match System.Type.GetTypeCode(typ) with
                        | TypeCode.Decimal -> value |> format "{0}m"
                        | TypeCode.String -> 
                            "\"" + Regex.Replace(value.ToString(), @"[^\u0020-\u007f]|""|\\", (fun (m : Match) -> 
                                    match m.Value.[0] with 
                                    | '\000' -> @"\0"
                                    | '\a' -> @"\a"
                                    | '\b' -> @"\a"
                                    | '\f' -> @"\f"
                                    | '\n' -> @"\n"
                                    | '\r' -> @"\r"
                                    | '\t' -> @"\t"
                                    | '\v' -> @"\v"
                                    | '\\' -> @"\\"
                                    | '"' -> @"\"""
                                    | ch  -> @"\u" + (int ch).ToString("x").PadLeft(4, '0'))) + "\""
                        | _ -> value |> format "{0}"
                    yield name, literal
            }
    }
    for asserts in rows do
        for prop, expected in asserts do
            printfn @"Assert.AreEqual(%s, e.Current.%s);" expected prop
        printfn @""

main()
