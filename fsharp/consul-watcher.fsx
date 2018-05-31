open System
open System.IO

(*-- 
You can download the consul executable from https://www.consul.io/downloads.html
Once you have downloaded you can watch key value updates and push it to log using the below script.

typically used like this:

consul.exe watch -http-addr="server_which_runs_consul:port" -type=keyprefix -prefix="key_prefix_to_watch_goes_here" "fsi consul_watcher.fsx"

consul_watcher.fsx in the above usage is this below script. This nicely parses and logs what has changed in the entire prefix blob.
--*)

(*-- utility functions start --*)

(*-- Function to decode from base 64 string --*)
let decodeBase64 text = 
    System.Text.ASCIIEncoding.ASCII.GetString(System.Convert.FromBase64String(text))

(* -- function to convert the string array to a map -- *)
let populate (keyvalues:string []) : Map<string,string> = 
    let mutable aMap = Map.empty
    let mutable key = ""
    let mutable value = ""
    for i in keyvalues do
        if i.Contains("Key") then
           key <- ((i.Split[|':'|]).[1]).Replace("\"", "")
        elif i.Contains("Value") then 
           value <- (decodeBase64 (((i.Split[|':'|]).[1]).Replace("\"", "")))           
        if key.Length > 0 then
           if value.Length > 0 then
              aMap <- aMap.Add(key,value)
              key <- ""
              value <- ""    
    aMap         

(*-- utility functions end --*)

let x = Console.ReadLine()
let oldFileName = "oldkv.txt"
let tempFolder = Path.GetTempPath()
let fileNameWithPath = sprintf "%s\\%s" tempFolder oldFileName
let y =  try File.ReadAllText(fileNameWithPath) with _ -> x
let kv = x.Split[|','|]
let oldKv = y.Split[|','|]

let mutable oldMap = populate oldKv
let mutable newMap = populate kv

let mutable hasChanged = false

let checkFunction (key: string) (value: string) : Unit = 
    let newValue = newMap.[key]
    if (String.Compare(newValue, value) <> 0) then
       hasChanged <- true 
       printfn "The value for the key: %s has been changed. New value is: %s. Old value was: %s" key newValue value
    ()

oldMap |> Map.iter checkFunction

if not hasChanged then
   printfn "- no changes -"   

//Finally Write back the new blob to old file   
File.WriteAllText(fileNameWithPath, x)


       