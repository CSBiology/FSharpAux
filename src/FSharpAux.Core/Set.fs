namespace FSharpAux


[<AutoOpen>]
module Set =    

    ///Computes the symmteric difference of the two sets, which is the set of elements which are in either of the sets and not in their intersection
    let symmetricDifference (set1:Set<'T>) (set2:Set<'T>) =

        let union = Set.union set1 set2
        let intersect = Set.intersect set1 set2

        Set.difference union intersect