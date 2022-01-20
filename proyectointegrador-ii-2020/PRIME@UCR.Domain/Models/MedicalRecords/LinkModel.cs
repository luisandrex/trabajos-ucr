using System;
using System.Collections.Generic;
using System.Text;

namespace PRIME_UCR.Domain.Models.MedicalRecords
{
    public class LinkModel
    {

        private string Text { get; set; }

        private int Page { get; set; }

        private bool Enabled { get; set; } = true;

        private bool Active { get; set; } = false;


        public LinkModel(int page, bool enabled, string text)
        {
            Page = page;
            Enabled = enabled;
            Text = text;

        }


        public void change_active_mode(bool mode)
        {
            this.Active = mode;
        }

        public bool is_it_active()
        {
            return this.Active;
        }

        public int get_page()
        {
            return this.Page;
        }

        public bool is_it_enabled()
        {
            return this.Enabled;
        }

        public string get_text_representation()
        {
            return this.Text;
        }

    }

}
