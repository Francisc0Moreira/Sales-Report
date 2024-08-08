using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SIOLoadDataService;
using SIOLoadDataService.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SIOLoadDataService
{
    class ReadData
    {

        private readonly OnContext _context;

        private IFormFile file { get; set; }

        public ReadData(OnContext context) => this._context = context;

        private int regionCount = 0;
        private int invoicesCount = 0;
        private int linesCount = 0;


        public List<Products>?  ReadSAFTProducts()
        {          

            try
            {
                List<Products> listaProdutos = new List<Products>();
                using (StreamReader streamReader = new StreamReader(file.OpenReadStream(), Encoding.GetEncoding("windows-1252")))
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(file.OpenReadStream());

                    XmlNodeList products = xmlDocument.GetElementsByTagName("Product");

                    foreach (XmlNode productNode in products)
                    {

                        // Extrair os dados do produto
                        string id = productNode.ChildNodes[1].InnerText;
                        string? descricao = productNode.ChildNodes[3].InnerText;
                        string? family = productNode.ChildNodes[2].InnerText;


                        // Adicionar o produto à lista
                        listaProdutos.Add(new Products { ProductId = id, Description = descricao, Family = family, Unity = "un" });
                    }

                }

                if(listaProdutos.Count > 0)
                {
                    return listaProdutos;
                }
                return null;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocorreu um erro ao ler os produtos do arquivo SAF-T: " + ex.Message);
                return null;
            }
        }

        public Company ReadSAFTCompany()
        {

            try
            {
                Company company = new Company();
                using (StreamReader streamReader = new StreamReader(file.OpenReadStream(), Encoding.GetEncoding("windows-1252")))
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(file.OpenReadStream());

                    XmlNodeList comp = xmlDocument.GetElementsByTagName("Header");

                    foreach (XmlNode compNode in comp)
                    {
                        XmlNode local = compNode.ChildNodes[6];

                        string Street = local.ChildNodes[0].InnerText;
                        string City = local.ChildNodes[1].InnerText;
                        string PostalCode = local.ChildNodes[2].InnerText;
                        string Country = local.ChildNodes[3].InnerText;

                        company = new Company
                        {
                            CompanyId = "1",
                            Name = compNode.ChildNodes[4].InnerText,
                            Street = Street,
                            City = City,
                            PostalCode = PostalCode,
                            Country = Country,
                            FiscalYear = compNode.ChildNodes[7].InnerText,
                            Nif = compNode.ChildNodes[13].InnerText,
                        };

                    }
                    

                }

                return company;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocorreu um erro ao ler as informações da empresa do arquivo SAF-T: " + ex.Message);
                return null;
            }
        }


        public List<Clients>? ReadSAFTClients()
        {

            try
            {
                List<Clients> listaClients = new List<Clients>();
                using (StreamReader streamReader = new StreamReader(file.OpenReadStream(), Encoding.GetEncoding("windows-1252")))
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(file.OpenReadStream());

                    XmlNodeList clients = xmlDocument.GetElementsByTagName("Customer");

                    foreach (XmlNode clientNode in clients)
                    {

                        
                        // Extrair os dados do produto
                        string Clientid = clientNode.ChildNodes[0].InnerText;
                        string? name = clientNode.ChildNodes[3].InnerText;
                        string? nif = clientNode.ChildNodes[2].InnerText;
                        string regionId = "R" + Clientid;

                        listaClients.Add(new Clients { ClientId = Clientid, Name = name, Nif = nif, LocalId = regionId });
                        ReadLoadSAFTRegion(clientNode.ChildNodes[5],name, regionId);
                        regionCount++;

                    }

                }

                if(listaClients.Count > 0) {
                    Console.WriteLine($"Regiões, {regionCount}, lidas com sucesso!");
                    return listaClients;
                }

                return null;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocorreu um erro ao ler os clientes do arquivo SAF-T: " + ex.Message);
                return null;
            }
        }

        public void ReadLoadSAFTRegion(XmlNode Clientlocal, string name, string regionId)
        {

            if(Clientlocal == null)
            {
                Console.WriteLine($"Não existe nenhum local para {name}");
            }

            try
            {       

                string? city = Clientlocal.ChildNodes[1].InnerText;
                string? street = Clientlocal.ChildNodes[0].InnerText;
                string? postalcode = Clientlocal.ChildNodes[2].InnerText;
                string? country = Clientlocal.ChildNodes[3].InnerText;

                Local local = new Local { City = city, Country = country, PostalCode = postalcode, Street = street, LocalId = regionId};

                LoadData loadData = new LoadData(_context);
                loadData.LoadRegion(local);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocorreu um erro ao ler as regiões do arquivo SAF-T: " + ex.Message);
            }
        }

        public void ReadSAFTInvoices()
        {

            try
            {
                using (StreamReader streamReader = new StreamReader(file.OpenReadStream(), Encoding.GetEncoding("windows-1252")))
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(file.OpenReadStream());

                    XmlNodeList sales = xmlDocument.GetElementsByTagName("Invoice");

                    foreach (XmlNode salesNode in sales)
                    {

                        int lastindex = salesNode.ChildNodes.Count - 1;

                        // Extrair os dados do produto
                        string salesId = salesNode.ChildNodes[0].InnerText;
                        string? ClientId = salesNode.ChildNodes[11].InnerText;
                        string? date = salesNode.ChildNodes[6].InnerText;
                        DateTime datetime = DateTime.Parse(date);
                        
                        XmlNode docNode = salesNode.ChildNodes[lastindex];

                        float? price = TransformPrice(docNode.ChildNodes[1].InnerText);

                        float? taxprice = TransformPrice(docNode.ChildNodes[2].InnerText);

                        Sales sale = new Sales { SalesId = salesId, ClientsId = ClientId, Date = datetime, Total = (decimal?)price, GrossTotal = (decimal?)taxprice};
                        LoadData loadData = new LoadData(_context);
                        loadData.LoadSales(sale);

                        for (int i = 0; i<lastindex;i++)
                        {
                            if (salesNode.ChildNodes[i].Name == "Line")
                            {
                                ReadLoadSAFTSalesLines(salesNode.ChildNodes[i], salesId);
                                linesCount++;
                            }

                        }

                        invoicesCount++;

                    }

                    Console.WriteLine($"Faturas, {invoicesCount}, adicionadas com sucesso");
                    Console.WriteLine($"Linhas de Fatura, {linesCount}, adicionadas com sucesso");

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocorreu um erro ao ler as faturas do arquivo SAF-T: " + ex.Message);
            }
        }

        public void ReadLoadSAFTSalesLines(XmlNode Line, string saleId)
        {

            if (Line == null)
            {
                Console.WriteLine($"Não existe nenhuma line para a sale {saleId}");
            }

            try
            {
                string? lineId = Line.ChildNodes[0].InnerText;
                string? productId = Line.ChildNodes[1].InnerText;
                string quantityText = Line.ChildNodes[3].InnerText;
                string[] quantityParts = quantityText.Split('.');
                int quantity = Int32.Parse(quantityParts[0]);

                float? productPrice = TransformPrice(Line.ChildNodes[5].InnerText);

                float? total = TransformPrice(Line.ChildNodes[8].InnerText);

                SalesLines salesLine = new SalesLines { Line = lineId, SalesId = saleId, ProductsId = productId, ProductPrice = (decimal?)productPrice, Total = (decimal?)total, Quantity = quantity};
                LoadData loadData = new LoadData(_context);
                loadData.LoadSaleLine(salesLine);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocorreu um erro ao ler as linhas de venda do arquivo SAF-T: " + ex.Message);
            }
        }


        private float TransformPrice(string price)
        {
            float totalprice = float.Parse(price.Replace('.', ','));
            return totalprice;
        }

        public void setSaftData(IFormFile file)
        {
            this.file = file;
        }


    }
}