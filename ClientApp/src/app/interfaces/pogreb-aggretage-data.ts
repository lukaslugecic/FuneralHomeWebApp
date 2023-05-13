import { IKorisnik } from "./korisnik-data";
import { IPogrebOpremaData } from "./pogreb-oprema-data";
import { IPogrebUslugeData } from "./pogreb-usluge-data";

export interface IPogrebAggretageData {
    id: number;
    smrtniSlucajId: number;
    datumPogreba: Date;
    kremacija: boolean;
    ukupnaCijena: number;
    pogrebOprema: IPogrebOpremaData[];
    pogrebUsluga: IPogrebUslugeData[];
    smrtniSlucaj: {
        id: number;
        korisnikID: number;
        imePok: string;
        prezimePok: string;
        oibpok: string;
        datumRodenjaPok: Date;
        datumSmrtiPok: Date;
    };
    korisnik: {
        id: number;
        ime: string;
        prezime: string;
        datumRodenja: string;
        adresa: string;
        oib: string;
        mail: string;
        lozinka: string;
        vrstaKorisnika: string
    };
}
