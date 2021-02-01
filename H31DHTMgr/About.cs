//======================================================================
//Copyright (C) 2009 ARTM
//All rights reserved

//Filename :About
//Created by Miao at  01/12/2009 15:20:10
//Description :����
//======================================================================
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Reflection;

namespace H31DHTMgr
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
            this.Text = String.Format("���� {0}", AssemblyTitle);
            this.labelProductName.Text = AssemblyProduct;
            this.labelVersion.Text = String.Format("�汾 {0}", AssemblyVersion);
            this.labelCopyright.Text = AssemblyCopyright;
            this.labelCompanyName.Text = AssemblyCompany;
        }

        #region �������Է�����

        public string AssemblyTitle
        {
            get
            {
                // ��ȡ�˳����ϵ����� Title ����
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                // ���������һ�� Title ����
                if (attributes.Length > 0)
                {
                    // ��ѡ���һ������
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    // ���������Ϊ�ǿ��ַ��������䷵��
                    if (titleAttribute.Title != "")
                        return titleAttribute.Title;
                }
                // ���û�� Title ���ԣ����� Title ����Ϊһ�����ַ������򷵻� .exe ������
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                // ��ȡ�˳��򼯵����� Description ����
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                // ��� Description ���Բ����ڣ��򷵻�һ�����ַ���
                if (attributes.Length == 0)
                    return "";
                // ����� Description ���ԣ��򷵻ظ����Ե�ֵ
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                // ��ȡ�˳����ϵ����� Product ����
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                // ��� Product ���Բ����ڣ��򷵻�һ�����ַ���
                if (attributes.Length == 0)
                    return "";
                // ����� Product ���ԣ��򷵻ظ����Ե�ֵ
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                // ��ȡ�˳����ϵ����� Copyright ����
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                // ��� Copyright ���Բ����ڣ��򷵻�һ�����ַ���
                if (attributes.Length == 0)
                    return "";
                // ����� Copyright ���ԣ��򷵻ظ����Ե�ֵ
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                // ��ȡ�˳����ϵ����� Company ����
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                // ��� Company ���Բ����ڣ��򷵻�һ�����ַ���
                if (attributes.Length == 0)
                    return "";
                // ����� Company ���ԣ��򷵻ظ����Ե�ֵ
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion

        private void okButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

    }
}