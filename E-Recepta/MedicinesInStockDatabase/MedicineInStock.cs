using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicinesInStockDatabase
{
    public class MedicineInStock : Medicine
    {
        private int _amount;
        private float _cost;
        private string _pharmacyId;

        public MedicineInStock(string id, string name, string manufacturer, string amount, string cost, string pharmacyId) : base(id, name, manufacturer)
        {
            _amount = int.Parse(amount);
            _cost = float.Parse(cost);
            _pharmacyId = pharmacyId;
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
