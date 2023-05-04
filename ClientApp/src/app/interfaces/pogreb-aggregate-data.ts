import { IOpremaData } from "./oprema-data";
import { ISmrtniSlucajData } from "./smrtnislucaj-data";
import { IUslugaData } from "./usluga-data";

export interface IPogrebAggregateData {
    id: number;
    datumPogreba: Date;
    kremacija: boolean;
    pogrebOprema: IOpremaData[];
    pogrebUsluga: IUslugaData[];
    smrtniSlucaj: ISmrtniSlucajData;
}