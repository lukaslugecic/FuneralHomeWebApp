export interface IPogrebOpremaData {
    oprema: {
        id: number;
        naziv: string;
        vrstaOpremeId: number;
        vrstaOpremeNaziv: string;
        slika: string;
        zalihaOpreme: number;
        cijena: number;
    },
    kolicina: number;
}