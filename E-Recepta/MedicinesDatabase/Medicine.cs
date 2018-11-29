using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicinesDatabase
{
    public class Medicine
    {
        private string _id;
        private string _name;
        private string _manufacturer;
        private float _refundRate;

        public Medicine(string id, string name, string manufacturer, string refundRate)
        {
            _id = id;
            _name = name;
            _manufacturer = manufacturer;
            _refundRate = float.Parse(refundRate);
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
            set
            {
                this._name = value;
            }
        }

        public string Manufacturer
        {
            get
            {
                return this._manufacturer;
            }
            set
            {
                this._manufacturer = value;
            }
        }

        public float RefundRate
        {
            get
            {
                return this._refundRate;
            }
            set
            {
                this._refundRate = value;
            }
        }
    }
}
