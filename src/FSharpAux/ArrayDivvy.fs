namespace FSharpAux

open System
open System.Collections
open System.Collections.Generic
open System.Runtime.InteropServices
open System.Diagnostics.Contracts

// ArrayDivvy
// ArraySegment
// ArrayDivvy

[<Serializable>]
[<Sealed>]
type private ArrayDivvyEnumerator<'T>internal(_array:'T[], _offset:int, _count:int) = 
    let _start   = _offset
    let _end     = _offset + _count
    let mutable _current = _start - 1
    interface IEnumerator<'T> with
        member this.MoveNext() =
            if (_current < _end) then
                _current <- _current + 1
                _current < _end
            else
                false
                        
        member this.get_Current() =
            _array.[_current]
        member IEnumerator.get_Current() =
            _array.[_current] :> obj
        member IEnumerator.Reset() = 
            _current <- _start - 1        
        member this.Dispose() = ()


[<Struct>]
[<CustomEquality;CustomComparison>]
type ArrayDivvy<'T when 'T : equality>(array:'T[], offset:int, count:int) =
      
    new (array:'T[]) = ArrayDivvy(array,0,array.Length)

    member this.Array = 
        if (obj.ReferenceEquals(array, null)) then
            raise (ArgumentNullException("array"))
        if (array.Length - offset < count) then
            raise (ArgumentException("Argument_InvalidOffLen"))
        array
    member this.Offset = 
        if (offset < 0) then
            raise (ArgumentOutOfRangeException("offset")) //, Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum")))
        offset
    member this.Count = 
        if (count < 0) then 
            raise (System.ArgumentOutOfRangeException("count")) //, Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum")))
        count

    override this.Equals(other) =
        match other with
            | :? ArrayDivvy<'T> as o -> (o.Array = array && o.Offset = offset && o.Count = count)
            | _ -> false

    override this.GetHashCode () =
        if (obj.ReferenceEquals(array, null)) then 0 else  (array.GetHashCode()  ^^^ offset ^^^ count)

    member this.Item  
        with get(index) =
            if (obj.ReferenceEquals(array, null)) then
                raise (ArgumentNullException("array"))
            if (index < 0 || index >=  count) then
                raise (ArgumentOutOfRangeException("index")) 
            array.[offset+index]
        and set index value =
            if (obj.ReferenceEquals(array, null)) then
                raise (ArgumentNullException("array"))
            if (index < 0 || index >=  count) then
                raise (ArgumentOutOfRangeException("index")) 
            array.[offset+index] <- value   

    member this.SetValue(value,index) = array.[offset+index] <- value   

    member this.GetSlice(start: int option, finish : int option) =
        let start = defaultArg start 0
        let finish = defaultArg finish (count - 1)
        if start < 0 || finish >= array.Length then
            raise (System.IndexOutOfRangeException("Index was outside the bounds of the array segment."))
        ArrayDivvy<'T>(array, offset + start, finish - start + 1)       
 
    member this.IndexOf item  = 
        if (obj.ReferenceEquals(array, null)) then
            raise (ArgumentNullException("array"))
        let index = System.Array.IndexOf<'T>(array, item, offset, count)
        if index >= 0 then index - offset else -1 

    member this.Contains(item) =
        if (obj.ReferenceEquals(array, null)) then
            raise (ArgumentNullException("array"))
        let index = System.Array.IndexOf<'T>(array, item, offset, count)
        index >= 0                        
    member this.CopyTo(cArray, arrayIndex) =
        if (obj.ReferenceEquals(array, null)) then
            raise (ArgumentNullException("array"))
        System.Array.Copy(array, offset, cArray, arrayIndex, count);
    
    // #region System.IComparable
    interface System.IComparable with

      member this.CompareTo other =
          match other with
          | :? ArrayDivvy<'T> as o ->  if (this.Equals(other)) then 1 else 0
          | _ -> invalidArg "other" "cannot compare values of different types"
    // #end region System.IComparable

    // #region IList<'T>
    interface IList<'T> with
        member this.Item  
            with get(index) =
                if (obj.ReferenceEquals(array, null)) then
                    raise (ArgumentNullException("array"))
                if (index < 0 || index >=  count) then
                    raise (ArgumentOutOfRangeException("index")) 
                array.[offset+index]
            and set index value =
                if (obj.ReferenceEquals(array, null)) then
                    raise (ArgumentNullException("array"))
                if (index < 0 || index >=  count) then
                    raise (ArgumentOutOfRangeException("index")) 
                array.[offset+index] <- value  
//        member this.Item
//            with get(index)             = this.Item(index)
//            and  set(index)(value)      = this.Item(index) <- value//array.[offset+index] <- value //this.Item(index) <- value
        member this.IndexOf item        = this.IndexOf item
        member this.Insert(index, item) = raise (NotSupportedException())
        member this.RemoveAt(index)     = raise (NotSupportedException())        
    // #end region IList<'T>

    // #region ICollection<'T>
    interface ICollection<'T> with 
        member this.IsReadOnly with get()      = false
        member this.Add(item)                  = raise (NotSupportedException())
        member this.Clear()                    = raise (NotSupportedException())
        member this.Remove(item)               = raise (NotSupportedException())
                                          
        member this.Count                      = this.Count
        member this.Contains(item)             = this.Contains(item)                      
        member this.CopyTo(cArray, arrayIndex) = this.CopyTo(cArray, arrayIndex)
    // #end region ICollection<'T>
                
    // #region IEnumerable<'T>           
    interface IEnumerable<'T> with
        member this.GetEnumerator() = 
            if (obj.ReferenceEquals(array, null)) then
                raise (ArgumentNullException("array"))
            new ArrayDivvyEnumerator<'T>(array,offset,count) :> IEnumerator<'T>

    interface IEnumerable with
        member this.GetEnumerator() = 
            if (obj.ReferenceEquals(array, null)) then
                raise (ArgumentNullException("array"))
            new ArrayDivvyEnumerator<'T>(array,offset,count) :> IEnumerator


 
    ///Applies the given function to each element of the divvy array.
    member this.Iter f =
        //checkNonNull "array" array
        for i=offset-1 to count-1 do
            f array.[i] 

    member this.Iteri f  =
        //checkNonNull "array" array
        let f = OptimizedClosures.FSharpFunc<_,_,_>.Adapt(f)
        let len = array.Length
        for i=0 to count-1 do
            f.Invoke(i, array.[i+offset])

    ///Applies a function to each element of the collection, threading an accumulator argument through the computation.
    member this.Fold<'T,'State> (f : 'State -> 'T -> 'State) (acc: 'State) =
        //checkNonNull "array" array
        let f = OptimizedClosures.FSharpFunc<_,_,_>.Adapt(f)
        let mutable state = acc 
        for i=offset-1 to count-1 do 
            state <- f.Invoke(state,array.[i])
        state


    member this.Map (f : 'T -> 'U) =
        //checkNonNull "array" array
        let res = Array.zeroCreate<'U> count
        for i=0 to count-1 do 
            res.[i] <- f array.[i+offset]
        ArrayDivvy res



    member this.Filter f = 
        //checkNonNull "array" array
        let res = new System.Collections.Generic.List<_>() // ResizeArray
        for i=offset-1 to count-1 do 
            let x = array.[i] 
            if f x then res.Add(x)
        res.ToArray() |> ArrayDivvy 



// https://github.com/fsharp/fsharp/blob/master/src/fsharp/FSharp.Core/array.fs

module ArrayDivvy = 

    ///Applies the given function to each element of the divvy array.
    let iter (f : 'T -> unit) (divvy : ArrayDivvy<'T>) = divvy.Iter f

    ///Applies a function to each element of the collection, threading an accumulator argument through the computation.
    let fold f s (divvy : ArrayDivvy<'T>) = divvy.Fold f s

    ///Returns the length of a mutable array segment.
    let inline count (divvy : ArrayDivvy<'T>) = divvy.Count  

    ///Builds a new divvy array whose elements are the results of applying the given function to each of the elements of the array.
    let map (f : 'T -> 'U) (divvy : ArrayDivvy<'T>) = divvy.Map f

    ///Returns a new collection containing only the elements of the collection for which the given predicate returns true.
    let filter (f : 'T -> bool) (divvy : ArrayDivvy<_>) = divvy.Filter f

//    ///Returns the first element for which the given function returns true. Return None if no such element exists.
//    let tryFind (f : 'T -> bool) (mas : ArrayDivvy<_>) = mas.TryFind f
//
//    ///Returns the first element for which the given function returns true. 
//    let find (f : 'T -> bool) (mas : ArrayDivvy<_>) = mas.Find f
//
//    ///Creates an array from the given mutable array segment.
//    let toArray (mas : ArrayDivvy<_>) = mas.ToArray () 


    //[<CompiledName("ZeroCreate")>]
    let zeroCreate count    = 
        //if count < 0 then invalidArg "count" (SR.GetString(SR.inputMustBeNonNegative))
        Array.zeroCreate<'T> count 
        |> ArrayDivvy

    //[<CompiledName("Create")>]
    let create (count:int) (x:'T) =
        //if count < 0 then invalidArg "count" (SR.GetString(SR.inputMustBeNonNegative))
        let array = Array.zeroCreate<'T> count
        for i = 0 to Operators.Checked.(-) count 1 do // use checked arithmetic here to satisfy FxCop
            array.[i] <- x
        ArrayDivvy array
