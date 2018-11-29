using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicinesInStockDatabase
{
    public class Medicine
    {
        private string _id;
        private string _name;
        private string _manufacturer;


        public Medicine(string id, string name, string manufacturer)
        {
            _id = id;
            _name = name;
            _manufacturer = manufacturer;
        }
        public string Id
        {
            get
            {
                return this._id;
            }

            set
            {
                this._id = value;
            }
        }

        public string Name
        {
            get
            {
                return this._name;
            }
        }

        public string Manufacturer
        {
            get
            {
                return this._manufacturer;
            }
        }
    }
}
