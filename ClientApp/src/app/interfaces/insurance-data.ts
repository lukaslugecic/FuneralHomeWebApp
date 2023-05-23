export interface IInsuranceData {
    Id: number;
    KorisnikId: number;
    Ime: string
    Prezime: string
    DatumUgovaranja: Date;
    PlacanjeNaRate: boolean;
    BrojRata: number;
    PaketOsiguranjaId: number;
    NazivPaketa: string;
}