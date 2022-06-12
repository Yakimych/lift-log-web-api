module LiftLog.Validation

open System.ComponentModel.DataAnnotations

type RpeAttribute() =
    inherit ValidationAttribute()
    
    let minValue = 6.5m
    let maxValue = 10m
    let step = 0.5m
    
    let allowedValues = [minValue .. step .. maxValue]
    let allowedValuesString = allowedValues |> List.map string |> String.concat ", "
    
    override this.IsValid(value: obj): bool =
        let rpeValue = value :?> decimal option
        match rpeValue with
        | None -> true 
        | Some rpe -> allowedValues |> List.contains rpe
        
    override this.FormatErrorMessage(name: string): string =
        sprintf "Only the following values are allowed for RPE: %s" allowedValuesString

type MaxLinksAttribute() =
    inherit ValidationAttribute()
    
    let maxNumberOfLinks = 3
    
    override this.IsValid(value: obj): bool =
        match value with
        | null -> true
        | nonNullValue ->
            let links = nonNullValue :?> obj seq
            links |> Seq.length <= maxNumberOfLinks
        
    override this.FormatErrorMessage(name: string): string =
        sprintf "A maximum of %d links is allowed." maxNumberOfLinks
