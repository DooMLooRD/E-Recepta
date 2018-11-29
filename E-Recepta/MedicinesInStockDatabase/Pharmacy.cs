using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicinesInStockDatabase
{
    public class Pharmacy
    {
        private string _id;
        private string _street;
        private string _city;

        public Pharmacy(string id, string street, string city)
        {
            this._id = id;
            this._street = street;
            this._city = city;
        }

        public string Id
        {
            get
            {
                return this._id;
            }
        }

        public string Street
        {
            get
            {
                return this._street;
            }
        }

        public string City
        {
            get
            {
                return this._city;
            }
        }
    }
}
