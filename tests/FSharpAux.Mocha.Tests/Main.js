import { equal } from 'assert';
import { String_trim } from "./fable/src/FSharpAux.Core/String.js";

describe('Mocha native', function () {
    describe('subtestlist', function () {
        it('should return -1 when the value is not present', function () {
            equal([1, 2, 3].indexOf(4), -1);
        });
        it ('test actual FSharpAux.Core func', function () {
            const actual = String_trim("      \tHi there!\n     ");
            equal(actual,"Hi there!")
        });
    });
});