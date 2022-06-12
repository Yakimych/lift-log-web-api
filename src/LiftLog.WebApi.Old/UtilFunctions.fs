module UtilFunctions

open System

let nullableToOption (nullableValue: Nullable<'a>) =
    if nullableValue.HasValue then
        Some nullableValue.Value
    else 
        None
        
let optionToNullable (optionalValue: Option<'a>): Nullable<'a> =
    match optionalValue with
    | Some value -> Nullable(value)
    | None -> Nullable()

