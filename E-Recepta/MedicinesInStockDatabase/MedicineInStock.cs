using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicinesInStockDatabase
{
    public class MedicineInStock
    {
        private string _id;
        private string _name;
        private string _manufacturer;
        private int _amount;
        private float _cost;
        private string _pharmacyId;

        public MedicineInStock(string id, string name, string manufacturer, string amount, string cost, string pharmacyId)
        {
            _id = id;
            _name = name;
            _manufacturer = manufacturer;
            _amount = int.Parse(amount);
            _cost = float.Parse(cost);
            _pharmacyId = pharmacyId;
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

        public int Amount
        {
            get
            {
                return this._amount;
            }
        }

        public float Cost
        {
            get
            {
                return this._cost;
            }
        } 

        public string PharmacyId
        {
            get
            {
                return this._pharmacyId;
            }
        }

    }
}
