namespace FSharpAux



//Auxiliary functions and Wrapper for System.DateTime
module DateTime =
    
    open System

    ///Returns true when the time represented by the input is based on local time, otherwise false.
    let isLocal (d:DateTime) =
        match d.Kind with
        |DateTimeKind.Local -> true
        | _ -> false
    
    ///Returns true when the time represented by the input is based on Coordinated Universal Time (UTC), otherwise false.
    let isUtc (d:DateTime) =
        match d.Kind with
        |DateTimeKind.Utc -> true
        | _ -> false
    
    ///Returns true when the time represented by the input is based on neither local time or Coordinated Universal Time (UTC), otherwise false.
    let isUnspecified (d:DateTime) =
        match d.Kind with
        |DateTimeKind.Unspecified -> true
        | _ -> false

    ///Converts the string representation of a date and time to its DateTime equivalent. Throws an exception if the string is not in correct format.
    let parse (s:string) = DateTime.Parse(s)

    ///Converts the specified string representation of a date and time to its DateTime equivalent. Returns Some value if the conversion succeeded, and otherwise None.
    let tryParse (s:string) = 
        match System.DateTime.TryParse(s) with
        |true,res -> Some res
        |_ -> None
    
    ///Converts the value of the current DateTime object to Coordinated Universal Time (UTC).
    let toUtc (d:DateTime) = d.ToUniversalTime()

    ///Converts the value of the current DateTime object to local time.
    let toLocal (d:DateTime) = d.ToLocalTime()

    ///Compares two instances of DateTime and returns an integer that indicates whether the first instance is earlier than, the same as, or later than the second instance.
    ///Before comparing DateTime objects, ensure that the objects represent times in the same time zone.
    let compare (timeA:DateTime) (toTimeB:DateTime) =
        timeA.CompareTo(toTimeB)
    
    ///Compares two instances of DateTime and returns an integer that indicates whether the first instance is earlier than, the same as, or later than the second instance. 
    ///Performs a conversion of the two input times to the specified DateTimeKind before comparing
    let compareWithConversion (commonFormat:DateTimeKind) (timeA:DateTime) (toTimeB:DateTime) =
        match commonFormat with
        |DateTimeKind.Local -> compare (toLocal timeA) (toLocal toTimeB)
        |DateTimeKind.Utc -> compare (toUtc timeA) (toUtc toTimeB)
        | _ -> compare timeA toTimeB

    ///Compares two instances of DateTime and returns true if the first instance is earlier than the second instance, and false otherwise. 
    let isEarlier (timeA:DateTime) (toTimeB:DateTime) =
        (compare timeA toTimeB) < 0

    ///Compares two instances of DateTime and returns true if the first instance is earlier than the second instance, and false otherwise. 
    ///Performs a conversion of the two input times to the specified DateTimeKind before comparing
    let isEarlierWithConversion (commonFormat:DateTimeKind) (timeA:DateTime) (toTimeB:DateTime) =
        match commonFormat with
        |DateTimeKind.Local -> isEarlier (toLocal timeA) (toLocal toTimeB)
        |DateTimeKind.Utc -> isEarlier (toUtc timeA) (toUtc toTimeB)
        | _ -> isEarlier timeA toTimeB

    ///Compares two instances of DateTime and returns true if the first instance is later than the second instance, and false otherwise. 
    let isLater (timeA:DateTime) (toTimeB:DateTime) =
        (compare timeA toTimeB) > 0

    ///Compares two instances of DateTime and returns true if the first instance is later than the second instance, and false otherwise. 
    ///Performs a conversion of the two input times to the specified DateTimeKind before comparing
    let isLaterWithConversion (commonFormat:DateTimeKind) (timeA:DateTime) (toTimeB:DateTime) =
        match commonFormat with
        |DateTimeKind.Local -> isLater (toLocal timeA) (toLocal toTimeB)
        |DateTimeKind.Utc -> isLater (toUtc timeA) (toUtc toTimeB)
        | _ -> isLater timeA toTimeB

    ///Compares two instances of DateTime and returns true if the first instance is the same as the second instance, and false otherwise. 
    let isSame (timeA:DateTime) (toTimeB:DateTime) =
        (compare timeA toTimeB) = 0

    ///Compares two instances of DateTime and returns true if the first instance is is the same as the second instance, and false otherwise. 
    ///Performs a conversion of the two input times to the specified DateTimeKind before comparing
    let isSameWithConversion (commonFormat:DateTimeKind) (timeA:DateTime) (toTimeB:DateTime) =
        match commonFormat with
        |DateTimeKind.Local -> isSame (toLocal timeA) (toLocal toTimeB)
        |DateTimeKind.Utc -> isSame (toUtc timeA) (toUtc toTimeB)
        | _ -> isSame timeA toTimeB
