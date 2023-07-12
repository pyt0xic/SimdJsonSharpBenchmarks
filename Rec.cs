namespace SimdJsonBench;

public sealed class Rec
{
    public Rec(string jobno, string invoiced, string invoiceno, string email, string oemail, string vin, string branch)
    {
        Jobno = jobno;
        Invoiced = invoiced;
        Invoiceno = invoiceno;
        Email = email;
        Oemail = oemail;
        Vin = vin;
        Branch = branch;
    }

    public string Jobno { get; set; }
    public string Invoiced { get; set; }
    public string Invoiceno { get; set; }
    public string Email { get; set; }
    public string Oemail { get; set; }
    public string Vin { get; set; }
    public string Branch { get; set; }
}
