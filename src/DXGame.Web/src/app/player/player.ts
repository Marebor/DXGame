import { Playroom } from '../playroom/playroom';
import { Guid } from "../common/guid";

export class Player {
    public id: string;
    public name: string;
    public playrooms: Playroom[];
    public activePlayroom: Playroom;
    public protectedWithPassword: boolean;

    constructor(name: string, protectedWithPassword: boolean) {
        this.id = Guid.newGuid();
        this.name = name;
        this.playrooms = [];
        this.protectedWithPassword = protectedWithPassword;
    }
}
