import { class_type } from "../../fable_modules/fable-library.4.0.6/Reflection.js";
import { fill, indexOf, equalsWith } from "../../fable_modules/fable-library.4.0.6/Array.js";
import { copyToArray, structuralHash, toIterator, getEnumerator, identityHash, defaultOf, equals } from "../../fable_modules/fable-library.4.0.6/Util.js";
import { Record } from "../../fable_modules/fable-library.4.0.6/Types.js";
import { defaultArg } from "../../fable_modules/fable-library.4.0.6/Option.js";

class ArrayDivvyEnumerator$1 {
    constructor(_array, _offset, _count) {
        this._array = _array;
        this._start = (_offset | 0);
        this._end = ((_offset + _count) | 0);
        this._current = ((this._start - 1) | 0);
    }
    "System.Collections.IEnumerator.MoveNext"() {
        const this$ = this;
        if (this$._current < this$._end) {
            this$._current = ((this$._current + 1) | 0);
            return this$._current < this$._end;
        }
        else {
            return false;
        }
    }
    "System.Collections.Generic.IEnumerator`1.get_Current"() {
        const this$ = this;
        return this$._array[this$._current];
    }
    "System.Collections.IEnumerator.get_Current"() {
        const IEnumerator = this;
        return IEnumerator._array[IEnumerator._current];
    }
    "System.Collections.IEnumerator.Reset"() {
        const IEnumerator = this;
        IEnumerator._current = ((IEnumerator._start - 1) | 0);
    }
    Dispose() {
    }
}

function ArrayDivvyEnumerator$1$reflection(gen0) {
    return class_type("FSharpAux.ArrayDivvyEnumerator`1", [gen0], ArrayDivvyEnumerator$1);
}

function ArrayDivvyEnumerator$1_$ctor_Z9715162(_array, _offset, _count) {
    return new ArrayDivvyEnumerator$1(_array, _offset, _count);
}

export class ArrayDivvy$1 extends Record {
    constructor(array, offset, count) {
        super();
        this.array = array;
        this.offset = (offset | 0);
        this.count = (count | 0);
    }
    Equals(other) {
        const this$ = this;
        if (other instanceof ArrayDivvy$1) {
            const o = other;
            return (equalsWith(equals, ArrayDivvy$1__get_Array(o), this$.array) && (ArrayDivvy$1__get_Offset(o) === this$.offset)) && (ArrayDivvy$1__get_Count(o) === this$.count);
        }
        else {
            return false;
        }
    }
    GetHashCode() {
        const this$ = this;
        return ((this$.array === defaultOf()) ? 0 : (identityHash(this$.array) ^ (this$.offset ^ this$.count))) | 0;
    }
    CompareTo(other) {
        const this$ = this;
        if (other instanceof ArrayDivvy$1) {
            return (equals(this$, other) ? 1 : 0) | 0;
        }
        else {
            throw new Error("cannot compare values of different types\\nParameter name: other");
        }
    }
    "System.Collections.Generic.IList`1.get_ItemZ524259A4"(index) {
        const this$ = this;
        if (this$.array === defaultOf()) {
            throw new Error("array");
        }
        if ((index < 0) ? true : (index >= this$.count)) {
            throw new Error("index");
        }
        return this$.array[this$.offset + index];
    }
    "System.Collections.Generic.IList`1.set_Item6570C449"(index, value) {
        const this$ = this;
        if (this$.array === defaultOf()) {
            throw new Error("array");
        }
        if ((index < 0) ? true : (index >= this$.count)) {
            throw new Error("index");
        }
        this$.array[this$.offset + index] = value;
    }
    "System.Collections.Generic.IList`1.IndexOf2B595"(item) {
        const this$ = this;
        return ArrayDivvy$1__IndexOf_2B595(this$, item) | 0;
    }
    "System.Collections.Generic.IList`1.Insert6570C449"(index, item) {
        throw new Error();
    }
    "System.Collections.Generic.IList`1.RemoveAtZ524259A4"(index) {
        throw new Error();
    }
    "System.Collections.Generic.ICollection`1.get_IsReadOnly"() {
        return false;
    }
    "System.Collections.Generic.ICollection`1.Add2B595"(item) {
        throw new Error();
    }
    "System.Collections.Generic.ICollection`1.Clear"() {
        throw new Error();
    }
    "System.Collections.Generic.ICollection`1.Remove2B595"(item) {
        throw new Error();
    }
    "System.Collections.Generic.ICollection`1.get_Count"() {
        const this$ = this;
        return ArrayDivvy$1__get_Count(this$) | 0;
    }
    "System.Collections.Generic.ICollection`1.Contains2B595"(item) {
        const this$ = this;
        return ArrayDivvy$1__Contains_2B595(this$, item);
    }
    "System.Collections.Generic.ICollection`1.CopyToZ3B4C077E"(cArray, arrayIndex) {
        const this$ = this;
        ArrayDivvy$1__CopyTo_Z2AA303D5(this$, cArray, arrayIndex);
    }
    GetEnumerator() {
        const this$ = this;
        if (this$.array === defaultOf()) {
            throw new Error("array");
        }
        return ArrayDivvyEnumerator$1_$ctor_Z9715162(this$.array, this$.offset, this$.count);
    }
    [Symbol.iterator]() {
        return toIterator(getEnumerator(this));
    }
    "System.Collections.IEnumerable.GetEnumerator"() {
        const this$ = this;
        if (this$.array === defaultOf()) {
            throw new Error("array");
        }
        return ArrayDivvyEnumerator$1_$ctor_Z9715162(this$.array, this$.offset, this$.count);
    }
}

export function ArrayDivvy$1$reflection(gen0) {
    return class_type("FSharpAux.ArrayDivvy`1", [gen0], ArrayDivvy$1, class_type("System.ValueType"));
}

export function ArrayDivvy$1_$ctor_Z9715162(array, offset, count) {
    return new ArrayDivvy$1(array, offset, count);
}

export function ArrayDivvy$1_$ctor_32EFB1E(array) {
    return ArrayDivvy$1_$ctor_Z9715162(array, 0, array.length);
}

export function ArrayDivvy$1__get_Array(this$) {
    if (this$.array === defaultOf()) {
        throw new Error("array");
    }
    if ((this$.array.length - this$.offset) < this$.count) {
        throw new Error("Argument_InvalidOffLen");
    }
    return this$.array;
}

export function ArrayDivvy$1__get_Offset(this$) {
    if (this$.offset < 0) {
        throw new Error("offset");
    }
    return this$.offset | 0;
}

export function ArrayDivvy$1__get_Count(this$) {
    if (this$.count < 0) {
        throw new Error("count");
    }
    return this$.count | 0;
}

export function ArrayDivvy$1__get_Item_Z524259A4(this$, index) {
    if (this$.array === defaultOf()) {
        throw new Error("array");
    }
    if ((index < 0) ? true : (index >= this$.count)) {
        throw new Error("index");
    }
    return this$.array[this$.offset + index];
}

export function ArrayDivvy$1__set_Item_6570C449(this$, index, value) {
    if (this$.array === defaultOf()) {
        throw new Error("array");
    }
    if ((index < 0) ? true : (index >= this$.count)) {
        throw new Error("index");
    }
    this$.array[this$.offset + index] = value;
}

export function ArrayDivvy$1__SetValue_Z521B3197(this$, value, index) {
    this$.array[this$.offset + index] = value;
}

export function ArrayDivvy$1__GetSlice_Z1D6DC7E0(this$, start, finish) {
    const start_1 = defaultArg(start, 0) | 0;
    const finish_1 = defaultArg(finish, this$.count - 1) | 0;
    if ((start_1 < 0) ? true : (finish_1 >= this$.array.length)) {
        throw new Error("Index was outside the bounds of the array segment.");
    }
    return ArrayDivvy$1_$ctor_Z9715162(this$.array, this$.offset + start_1, (finish_1 - start_1) + 1);
}

export function ArrayDivvy$1__IndexOf_2B595(this$, item) {
    if (this$.array === defaultOf()) {
        throw new Error("array");
    }
    const index = indexOf(this$.array, item, this$.offset, this$.count, {
        Equals: equals,
        GetHashCode: structuralHash,
    }) | 0;
    if (index >= 0) {
        return (index - this$.offset) | 0;
    }
    else {
        return -1;
    }
}

export function ArrayDivvy$1__Contains_2B595(this$, item) {
    if (this$.array === defaultOf()) {
        throw new Error("array");
    }
    return indexOf(this$.array, item, this$.offset, this$.count, {
        Equals: equals,
        GetHashCode: structuralHash,
    }) >= 0;
}

export function ArrayDivvy$1__CopyTo_Z2AA303D5(this$, cArray, arrayIndex) {
    if (this$.array === defaultOf()) {
        throw new Error("array");
    }
    copyToArray(this$.array, this$.offset, cArray, arrayIndex, this$.count);
}

export function ArrayDivvy$1__Iter_5028453F(this$, f) {
    for (let i = this$.offset - 1; i <= (this$.count - 1); i++) {
        f(this$.array[i]);
    }
}

export function ArrayDivvy$1__Iteri_Z3561EC8B(this$, f) {
    const f_1 = f;
    const len = this$.array.length | 0;
    for (let i = 0; i <= (this$.count - 1); i++) {
        f_1(i, this$.array[i + this$.offset]);
    }
}

export function ArrayDivvy$1__Fold(this$, f, acc) {
    const f_1 = f;
    let state = acc;
    for (let i = this$.offset - 1; i <= (this$.count - 1); i++) {
        state = f_1(state, this$.array[i]);
    }
    return state;
}

export function ArrayDivvy$1__Map_Z6313B9F2(this$, f) {
    const res = fill(new Array(this$.count), 0, this$.count, null);
    for (let i = 0; i <= (this$.count - 1); i++) {
        res[i] = f(this$.array[i + this$.offset]);
    }
    return ArrayDivvy$1_$ctor_32EFB1E(res);
}

export function ArrayDivvy$1__Filter_Z1D55A0D7(this$, f) {
    const res = [];
    for (let i = this$.offset - 1; i <= (this$.count - 1); i++) {
        const x = this$.array[i];
        if (f(x)) {
            void (res.push(x));
        }
    }
    return ArrayDivvy$1_$ctor_32EFB1E(res.slice());
}

export function ArrayDivvy_iter(f, divvy) {
    ArrayDivvy$1__Iter_5028453F(divvy, f);
}

export function ArrayDivvy_fold(f, s, divvy) {
    return ArrayDivvy$1__Fold(divvy, f, s);
}

export function ArrayDivvy_map(f, divvy) {
    return ArrayDivvy$1__Map_Z6313B9F2(divvy, f);
}

export function ArrayDivvy_filter(f, divvy) {
    return ArrayDivvy$1__Filter_Z1D55A0D7(divvy, f);
}

export function ArrayDivvy_zeroCreate(count) {
    return ArrayDivvy$1_$ctor_32EFB1E(fill(new Array(count), 0, count, null));
}

export function ArrayDivvy_create(count, x) {
    const array = fill(new Array(count), 0, count, null);
    for (let i = 0; i <= (count - 1); i++) {
        array[i] = x;
    }
    return ArrayDivvy$1_$ctor_32EFB1E(array);
}

