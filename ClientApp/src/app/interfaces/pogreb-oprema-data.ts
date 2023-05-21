export interface IPogrebOpremaData {
    opremaUsluga: {
        id: number;
        naziv: string;
        vrstaOpremeUslugeId: number;
        vrstaOpremeUslugeNaziv: string;
        jeOprema: boolean;
        slika: string | null;
        zaliha: number | null;
        opis: string | null;
        jedinicaMjereNaziv: string,
        cijena: number;
    },
    kolicina: number;
}