import { empty, fold, filter, map, cons, find, tryFind } from "../../fable_modules/fable-library.4.0.6/List.js";
import { equals } from "../../fable_modules/fable-library.4.0.6/Util.js";
import { some } from "../../fable_modules/fable-library.4.0.6/Option.js";

export function getNodeFromAdjacencyNode(adjNode_, adjNode__1) {
    return adjNode_;
}

export function getEdgesFromAdjacencyNode(adjNode_, adjNode__1) {
    return adjNode__1;
}

export function nodeId(v_, v__1) {
    let copyOfStruct = v_;
    return copyOfStruct.Id;
}

export function tryGetAdjacencyNode(v, g) {
    return tryFind((V) => {
        const V_1 = V;
        return equals(nodeId(V_1[0], V_1[1]), v);
    }, g);
}

export function getAdjacencyNode(v, g) {
    return find((V) => {
        const V_1 = V;
        return equals(nodeId(V_1[0], V_1[1]), v);
    }, g);
}

export function tryGetNodeById(v, g) {
    const matchValue = tryGetAdjacencyNode(v, g);
    if (matchValue == null) {
        return void 0;
    }
    else {
        return some(matchValue[0]);
    }
}

export function tryGetEdges(v, g) {
    const matchValue = tryGetAdjacencyNode(v, g);
    if (matchValue == null) {
        return void 0;
    }
    else {
        return matchValue[1];
    }
}

export function addAdjacencyNode(adjNode_, adjNode__1, g) {
    let copyOfStruct;
    const adjNode = [adjNode_, adjNode__1];
    if (tryGetNodeById((copyOfStruct = getNodeFromAdjacencyNode(adjNode[0], adjNode[1]), copyOfStruct.Id), g) == null) {
        return cons(adjNode, g);
    }
    else {
        return g;
    }
}

export function addEdge(e, g) {
    return map((tupledArg) => {
        let copyOfStruct, copyOfStruct_1;
        const v = tupledArg[0];
        const adj = tupledArg[1];
        if (equals((copyOfStruct = v, copyOfStruct.Id), (copyOfStruct_1 = e, copyOfStruct_1.SourceId))) {
            return [v, cons(e, adj)];
        }
        else {
            return [v, adj];
        }
    }, g);
}

export function removeEdge(eId, g) {
    return map((tupledArg) => [tupledArg[0], filter((e) => {
        let copyOfStruct;
        return !equals((copyOfStruct = e, copyOfStruct.Id), eId);
    }, tupledArg[1])], g);
}

export function removeVertex(nodeId_1, g) {
    return fold((s$0027, tupledArg) => {
        let copyOfStruct;
        const v = tupledArg[0];
        if (equals((copyOfStruct = v, copyOfStruct.Id), nodeId_1)) {
            return s$0027;
        }
        else {
            return cons([v, filter((x) => {
                let copyOfStruct_1;
                return !equals((copyOfStruct_1 = x, copyOfStruct_1.TargetId), nodeId_1);
            }, tupledArg[1])], s$0027);
        }
    }, empty(), g);
}

