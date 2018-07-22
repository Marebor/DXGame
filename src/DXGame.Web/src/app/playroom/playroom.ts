import { Guid } from "../common/guid";

export class Playroom {
    public id: string;
    public name: string;
    public owner: string;
    public isPrivate: boolean;
    public players: string[];
    public completedGames: string[];
    public activeGame: string;
    
    constructor(name: string, owner: string, isPrivate: boolean, password: string) {
        this.id = Guid.newGuid();
        this.name = name;
        this.owner = owner;
        this.isPrivate = isPrivate;
    }
}
