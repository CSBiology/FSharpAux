import { newGuid, parse } from "../../fable_modules/fable-library.4.0.6/Guid.js";

export const empty = "00000000-0000-0000-0000-000000000000";

export function ofString(s) {
    return parse(s);
}

export const create = newGuid();

