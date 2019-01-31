using System;
using System.Collections.Generic;
using ZipApi.Entities;
using ZipApi.Models;
using System.Linq;
using CsvHelper;
using System.Net.Http;
using System.IO;
using ZipApi.Interfaces;

namespace ZipApi.Services
{
    public class DataLoadingService : IDataLoadingService
    {
        private readonly string CbsaDataUrl = "https://s3.amazonaws.com/peerstreet-static/engineering/zip_to_msa/zip_to_cbsa.csv";
        private readonly string MsaDataUrl = "https://s3.amazonaws.com/peerstreet-static/engineering/zip_to_msa/cbsa_to_msa.csv";

        private readonly ZipDbContext _context;

        public DataLoadingService(ZipDbContext context)
        {
            _context = context;
        }

        public bool LoadData()
        {
            LoadCbsaData();
            LoadMsaData();
            return true;
        }

        public void LoadMsaData()
        {
            //Load Msa Data
            var csvReader = GetReaderForUrl(MsaDataUrl);

            var msaRecords = new List<MsaData>();
            using (var csv = csvReader)
            {
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    try
                    {
                        var record = csv.GetRecord<MsaData>();
                        msaRecords.Add(record);
                    }
                    catch (Exception e)
                    {
                        var x = e;
                        csv.Read();
                    }
                }
            }

            var msas = msaRecords.Select(x => new MsaEntity
            {
                Cbsa = x.CBSA,
                Mdiv = x.MDIV,
                Lsad = x.LSAD,
                PopEstimate2014 = x.POPESTIMATE2014,
                PopEstimate2015 = x.POPESTIMATE2015,
                Name = x.NAME
            })
            .ToList();

            _context.RemoveRange(_context.Msas.ToList());
            _context.Msas.AddRange(msas);

            _context.SaveChanges();
        }

        public void LoadCbsaData()
        {
            //Load Cbsa Data
            var csvReader = GetReaderForUrl(CbsaDataUrl);
            var records = csvReader.GetRecords<CbsaData>().ToList();

            var cbsas = records.Select(x => new CbsaEntity
            {
                Zip = x.ZIP,
                Cbsa = x.CBSA
            })
            .ToList();

            _context.RemoveRange(_context.Cbsas.ToList());
            _context.Cbsas.AddRange(cbsas);

            _context.SaveChanges();
        }


        private CsvReader GetReaderForUrl(string url)
        {
            var config = new CsvHelper.Configuration.Configuration
            {
                HeaderValidated = null,
                MissingFieldFound = null,
            };

            HttpClient client = new HttpClient();
            var net = new System.Net.WebClient();
            var data = net.DownloadData(url);
            var content = new MemoryStream(data);
            TextReader reader = new StreamReader(content);

            var csvReader = new CsvReader(reader, config);
            return csvReader;
        }
    }
}
