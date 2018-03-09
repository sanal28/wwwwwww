﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETLCenterWebServices.Common_Library
{
    public class Constants
    {
        //Login and Forget Password
        public const string connection = "Connection";
        public const string UspEmployeeValidLogin = "UspEmployeeValidLogin";
        public const string UspEmpForgotPassword = "UspEmpForgotPassword";
        public const string EmpEmail = "@EmpEmail";
        public const string EmpPassword = "@EmpPassword";
        public const string userId = "@ID_Employee";
        public const string UspEmployeeResetPassword = "UspEmployeeResetPassword";


        //Support
        public const string UspELTSupportUpdate = "UspELTSupportUpdate";
        public const string ID_ELTSupport = "@ID_ELTSupport";
        public const string ESEmail = "@ESEmail";
        public const string ESName = "@ESName";
        public const string ESMessage = "@ESMessage";
        public const string Action = "@Action";

        // Employee Creation
        public const string ID_Employee = "@ID_Employee";
        public const string EmpName = "@EmpName";
        //public const string EmpCreationPassword = "@EmpPassword";
       // public const string EmpEmailCreation="@EmpEmail";
        public const string EmpIsActive = "@EmpIsActive";
        public const string EmpIsDelete = "@EmpIsDelete";
        public const string EnteredBy = "@EnteredBy";
        public const string ModifiedBy = "@ModifiedBy";
        public const string UspEmployeeUpdate = "UspEmployeeUpdate";
        public const string UspEmployeeSelect = "UspEmployeeSelect";


    }
}