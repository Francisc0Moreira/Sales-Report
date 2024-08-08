export class Company {

  constructor(
    public companyId: string,
    public name: string,
    public nif?: string,
    public country?: string,
    public city?: string,
    public postalCode?: string,
    public street?: string,
    public fiscalYear?: string
  ) { }
}
