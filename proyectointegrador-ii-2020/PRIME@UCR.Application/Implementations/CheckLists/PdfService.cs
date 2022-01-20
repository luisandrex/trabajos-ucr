using System;
using System.Collections.Generic;
using System.Text;
using PRIME_UCR.Application.Services.CheckLists;
using PRIME_UCR.Application.DTOs.CheckLists;
using System.Threading.Tasks;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using Syncfusion.Drawing;
using System.IO;
using Syncfusion.Pdf.Tables;

namespace PRIME_UCR.Application.Implementations.CheckLists
{
    public class PdfService : IPdfService
    {
        private PdfPage page;
        private PdfLayoutResult result;
        private PdfLayoutFormat format;
        private int paragraphAfterSpacing;

        public PdfService()
        {
            paragraphAfterSpacing = 8;
        }

        public MemoryStream GenerateIncidentPdf(PdfModel information)
        {
            if (information.Incident != null) 
            {
                using (PdfDocument pdfDocument = new PdfDocument()) 
                {

                    // Add page to the PDF document
                    page = pdfDocument.Pages.Add();

                    // Setting the header in the pdf.
                    PdfPageTemplateElement header = SetHeader(pdfDocument, "Caja Costarricense del Seguro Social – CEACO - Unidad COV19\nNúmeros Telefónicos: 2539-1313 / 8910-6105");

                    pdfDocument.Template.Top = header;

                    // Font used for the main title.
                    PdfStandardFont titleFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 16, PdfFontStyle.Bold);

                    // Format to follow for everything added to PDF.
                    format = new PdfLayoutFormat();
                    format.Layout = PdfLayoutType.Paginate;

                    // Adding the main title.
                    PdfTextElement title = new PdfTextElement("PRESENTACIÓN DE PACIENTE A UNIDAD COV19", titleFont, PdfBrushes.Blue);
                    title.StringFormat = new PdfStringFormat();
                    title.StringFormat.Alignment = PdfTextAlignment.Center;
                    result = title.Draw(page, new RectangleF(0, 0 + paragraphAfterSpacing, page.GetClientSize().Width, page.GetClientSize().Height), format);

                    AddGeneralInformation(pdfDocument, information);
                    AddCovidNote(pdfDocument, information);
                    AddPatientInformation(pdfDocument);
                    AddVitalSigns(pdfDocument);
                    AddBreathingInformation(pdfDocument);
                    AddLaboratoriesAndCabinet(pdfDocument);
                    AddChestRX(pdfDocument);
                    AddArterialGases(pdfDocument);
                    AddAnalisis(pdfDocument);
                    AddAnnex1(pdfDocument);
                    AddIncidentMultimedia(pdfDocument, information);

                    using (MemoryStream stream = new MemoryStream())
                    {
                        // Saving the PDF document into the stream
                        pdfDocument.Save(stream);
                        // Closing the PDF document
                        pdfDocument.Close(true);
                        return stream;

                    }
                }
            } else
            {
                return null;
            }
        }

        public PdfPageTemplateElement SetHeader(PdfDocument doc, string title)
        {
            RectangleF rect = new RectangleF(0, 0, doc.Pages[0].GetClientSize().Width, 50);

            // Creating a template for the header.
            PdfPageTemplateElement header = new PdfPageTemplateElement(rect);

            // Font used in the header.
            PdfStandardFont pdfFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 12, PdfFontStyle.Regular);

            // Adding the header text.
            string text = title;
            header.Graphics.DrawString(text, pdfFont, PdfBrushes.Black, new RectangleF(0, 20, doc.Pages[0].GetClientSize().Width, 50));

            return header;
        }

        private void DrawText(string text, PdfStandardFont font, float position)
        {
            PdfTextElement title = new PdfTextElement(text, font, PdfBrushes.Black)
            {
                StringFormat = new PdfStringFormat()
            };
            title.StringFormat.Alignment = PdfTextAlignment.Center;
            result = title.Draw(result.Page, new RectangleF(0, position, page.GetClientSize().Width, page.GetClientSize().Height), format);
        }

        private PdfGrid CreateTable(int columns, int rows, List<string> headers, List<List<string>> information, int fontSize)
        {
            // Margin used in the cells.
            int cellMargin = 8;
            // Font used in the table.
            PdfStandardFont contentFont = new PdfStandardFont(PdfFontFamily.TimesRoman, fontSize);

            PdfGrid pdfGrid = new PdfGrid();
            pdfGrid.Style.CellPadding.Left = cellMargin;
            pdfGrid.Style.CellPadding.Right = cellMargin;

            // Applying built-in style to the PDF grid
            pdfGrid.ApplyBuiltinStyle(PdfGridBuiltinStyle.GridTable4Accent1);

            // Adding the columns to the table
            pdfGrid.Columns.Add(columns);

            // If there are headers.
            if (headers != null)
            {
                PdfGridRow row = pdfGrid.Rows.Add();
                FillRow(row, columns, headers, true);
            }

            // For every row of information to be put in the table.
            for (int index = 0; index < rows; index++)
            {
                PdfGridRow row = pdfGrid.Rows.Add();
                FillRow(row, columns, information[index], false);
            }

            // Setting the table font.
            pdfGrid.Style.Font = contentFont;

            return pdfGrid;
        }

        private PdfGridRow FillRow(PdfGridRow row, int columns, List<string> information, bool header)
        {
            // Filling one row of the table
            for (int index = 0; index < columns; index++)
            {
                row.Cells[index].Value = information[index];
                // If its a header, center the text.
                if (header)
                {
                    row.Cells[index].StringFormat.Alignment = PdfTextAlignment.Center;
                }
            }
            return row;
        }

        private void AddGeneralInformation(PdfDocument doc, PdfModel generalInformation)
        {
            PdfStandardFont contentTitleFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 12, PdfFontStyle.Bold);
            DrawText("INFORMACIÓN GENERAL", contentTitleFont, result.Bounds.Bottom + paragraphAfterSpacing);

            PdfTextElement medicData = new PdfTextElement("Datos del Médico", contentTitleFont, PdfBrushes.Black);
            result = medicData.Draw(page, new RectangleF(0, result.Bounds.Bottom + paragraphAfterSpacing, page.GetClientSize().Width, page.GetClientSize().Height), format);

            List<string> headers = new List<string>
            {
                "Nombre Completo",
                "Número telefónico",
                "Lugar de donde llama"
            };

            string doctorName = "";
            string telephoneNumber = " ";
            string originName = "";
            if (generalInformation.AssignedMembers != null)
            {
                doctorName = generalInformation.AssignedMembers.Coordinator.NombreCompleto;
                if (generalInformation.Incident.Origin != null)
                {
                    originName = generalInformation.Incident.Origin.DisplayName;
                }
            }

            List<List<string>> information = new List<List<string>>();
            List<string> rowInformation = new List<string>
            {
                doctorName,
                telephoneNumber,
                originName
            };
            information.Add(rowInformation);

            PdfGrid pdfGrid = CreateTable(3, 1, headers, information, 12);

            result = pdfGrid.Draw(result.Page, new PointF(0, result.Bounds.Bottom + paragraphAfterSpacing));

            PdfTextElement patientData = new PdfTextElement("Datos del Paciente", contentTitleFont, PdfBrushes.Black);
            result = patientData.Draw(page, new RectangleF(0, result.Bounds.Bottom + paragraphAfterSpacing, page.GetClientSize().Width, page.GetClientSize().Height), format);

            headers = new List<string>
            {
                "Nombre Completo",
                "ID del Paciente",
                "Fecha de Nacimiento",
                "Edad"
            };

            string fullName = "";
            string personId = "";
            string birthDate = " ";
            string age = " ";
            
            if (generalInformation.Patient != null)
            {
                fullName = generalInformation.Patient.NombreCompleto;
                personId = generalInformation.Incident.MedicalRecord.CedulaPaciente;
                birthDate = "No ingresada en el sistema.";
                age = "No ingresada en el sistema.";
                if (generalInformation.Patient.FechaNacimiento != null)
                {
                    DateTime patientBirth = (DateTime)generalInformation.Patient.FechaNacimiento;
                    birthDate = patientBirth.ToString("dd/MM/yyyy");
                    int patientAge = DateTime.Now.Year - patientBirth.Year;
                    age = patientAge.ToString() + " años.";
                }
            }
            information = new List<List<string>>();
            rowInformation = new List<string>
            {
                fullName,
                personId,
                birthDate,
                age
            };
            information.Add(rowInformation);

            pdfGrid = CreateTable(4, 1, headers, information, 12);

            // pdfGrid.Columns[3].Width = 50;

            result = pdfGrid.Draw(result.Page, new PointF(0, result.Bounds.Bottom + paragraphAfterSpacing));
        }

        private void AddCovidNote(PdfDocument doc, PdfModel generalInformation)
        {
            PdfStandardFont contentTitleFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 12, PdfFontStyle.Bold);
            DrawText("NOTA TRASLADO UNIDAD COVID – NOTA MÉDICA", contentTitleFont, result.Bounds.Bottom + paragraphAfterSpacing);

            List<string> headers = new List<string>
            {
                "Antecedente Personales\nPatológicos:",
                "Antecedente Personales\nNo Patológicos:",
                "Sí",
                "No"
            };

            string background = " ";
            if (generalInformation.Background != null && generalInformation.Background.Count != 0)
            {
                background = "";
                for (int index = 0; index < generalInformation.Background.Count; index++)
                {
                    background += generalInformation.Background[index].ListaAntecedentes.NombreAntecedente;
                    background += ".\n";
                }
            }
            string treatment = " ";
            if (generalInformation.ChronicConditions != null && generalInformation.ChronicConditions.Count != 0)
            {
                treatment = "";
                for (int index = 0; index < generalInformation.ChronicConditions.Count; index++)
                {
                    treatment += generalInformation.ChronicConditions[index].ListaPadecimiento.NombrePadecimiento;
                    treatment += ".\n";
                }
            }
            List<List<string>> information = new List<List<string>>();
            List<string> rowInformation = new List<string>
            {
                background,
                "Tabaco",
                "",
                ""
            };
            information.Add(rowInformation);
            rowInformation = new List<string>
            {
                "",
                "Alcohol",
                "",
                ""
            };
            information.Add(rowInformation);
            rowInformation = new List<string>
            {
                "",
                "Drogas",
                "",
                ""
            };
            information.Add(rowInformation);
            rowInformation = new List<string>
            {
                "",
                "Alergias a Medicamentos",
                "",
                ""
            };
            information.Add(rowInformation);
            rowInformation = new List<string>
            {
                "",
                "Otros",
                "",
                ""
            };
            information.Add(rowInformation);

            PdfGrid pdfGrid = CreateTable(headers.Count, information.Count, headers, information, 12);

            // Setting manually Column Spans
            pdfGrid.Rows[1].Cells[0].RowSpan = 5;
            pdfGrid.Rows[5].Cells[1].ColumnSpan = 3;
            // Setting the width manually
            pdfGrid.Columns[1].Width = 150;
            pdfGrid.Columns[2].Width = 35;
            pdfGrid.Columns[3].Width = 35;

            result = pdfGrid.Draw(result.Page, new PointF(0, result.Bounds.Bottom + paragraphAfterSpacing));

            headers = new List<string>
            {
                "Tratamiento Crónico:",
                "AQX de Importancia:"
            };

            information = new List<List<string>>();
            rowInformation = new List<string>
            {
                treatment,
                ""
            };
            information.Add(rowInformation);

            pdfGrid = CreateTable(2, 1, headers, information, 12);

            result = pdfGrid.Draw(result.Page, new PointF(0, result.Bounds.Bottom + paragraphAfterSpacing));

        }

        private void AddPatientInformation(PdfDocument doc)
        {
            PdfStandardFont contentTitleFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 12, PdfFontStyle.Bold);
            DrawText("INFORMACIÓN DEL PACIENTE - COVID-19", contentTitleFont, result.Bounds.Bottom + paragraphAfterSpacing);

            List<string> headers = new List<string>
            {
                "Tratamiento Crónico:",
                "",
                "AQX de Importancia:",
                "",
                ""
            };

            List<List<string>> information = new List<List<string>>();
            List<string> rowInformation = new List<string>
            {
                "Positivo",
                "Nexo",
                "Fecha Diagnóstico",
                "Fecha: Inicio Síntomas",
                "Días de Evolución"
            };
            information.Add(rowInformation);
            rowInformation = new List<string>
            {
                " ",
                " ",
                " ",
                " ",
                " "
            };
            information.Add(rowInformation);

            PdfGrid pdfGrid = CreateTable(headers.Count, information.Count, headers, information, 12);

            // Setting manually Column Spans.
            pdfGrid.Rows[0].Cells[0].ColumnSpan = 2;
            pdfGrid.Rows[0].Cells[2].ColumnSpan = 3;
            for (int index = 0; index < rowInformation.Count; index++)
            {
                pdfGrid.Rows[1].Cells[index].StringFormat.Alignment = PdfTextAlignment.Center;
                pdfGrid.Rows[1].Cells[index].Style.BackgroundBrush = PdfBrushes.Aqua;
            }

            result = pdfGrid.Draw(result.Page, new PointF(0, result.Bounds.Bottom + paragraphAfterSpacing));

            headers = new List<string>
            {
                "Padecimiento Actual / MC:"
            };
            
            information = new List<List<string>>();
            rowInformation = new List<string>
            {
                " "
            };
            information.Add(rowInformation);

            pdfGrid = CreateTable(rowInformation.Count, information.Count, headers, information, 12);

            result = pdfGrid.Draw(result.Page, new PointF(0, result.Bounds.Bottom + paragraphAfterSpacing));
        }

        private void AddVitalSigns(PdfDocument doc)
        {
            PdfStandardFont contentTitleFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 12, PdfFontStyle.Bold);
            DrawText("SIGNOS VITALES", contentTitleFont, result.Bounds.Bottom + paragraphAfterSpacing);

            List<List<string>> information = new List<List<string>>();
            List<string> rowInformation = new List<string>
            {
                "FR: ___ RPM",
                "FC: ___ LPM",
                "T: ___ C ",
                "SPO2: ___%",
                "PA: ___ MMHG",
                "GLASGOW: ___",
                "GLI: ___"
            };
            information.Add(rowInformation);

            PdfGrid pdfGrid = CreateTable(rowInformation.Count, information.Count, null, information, 10);

            // Cell width is adjusted based on text width.
            pdfGrid.Style.AllowHorizontalOverflow = true;

            result = pdfGrid.Draw(result.Page, new PointF(0, result.Bounds.Bottom + paragraphAfterSpacing));
        }

        private void AddBreathingInformation(PdfDocument doc)
        {
            PdfStandardFont contentTitleFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 12, PdfFontStyle.Bold);
            DrawText("RESPIRANDO CON", contentTitleFont, result.Bounds.Bottom + paragraphAfterSpacing);

            List<string> headers = new List<string>
            {
                "Ventilador",
                "",
                "Parámetros\n(Flujo y FiO2)",
                "Ventilador",
                "",
                "Parámetros\n(Flujo y FiO2)"
            };
            List<List<string>> information = new List<List<string>>();
            List<string> rowInformation = new List<string>
            {
                "( )",
                "NC",
                "",
                "( )",
                "CAF",
                "",
            };
            information.Add(rowInformation);
            rowInformation = new List<string>
            {
                "( )",
                "VENTURY",
                "",
                "( )",
                "VMNI",
                "",
            };
            information.Add(rowInformation);
            rowInformation = new List<string>
            {
                "( )",
                "RESERVORIO",
                "",
                "( )",
                "TET",
                "",
            };
            information.Add(rowInformation);

            PdfGrid pdfGrid = CreateTable(rowInformation.Count, information.Count, headers, information, 12);

            // Setting manually Column Spans
            pdfGrid.Rows[0].Cells[0].ColumnSpan = 2;
            pdfGrid.Rows[0].Cells[3].ColumnSpan = 2;

            // Setting the width manually
            pdfGrid.Columns[0].Width = 30;
            pdfGrid.Columns[3].Width = 30;

            result = pdfGrid.Draw(result.Page, new PointF(0, result.Bounds.Bottom + paragraphAfterSpacing), format);
        }

        private void AddLaboratoriesAndCabinet(PdfDocument doc)
        {
            PdfStandardFont contentTitleFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 12, PdfFontStyle.Bold);
            DrawText("LABORATORIOS Y GABINETE", contentTitleFont, result.Bounds.Bottom + paragraphAfterSpacing);

            List<List<string>> information = new List<List<string>>();
            List<string> rowInformation = new List<string>
            {
                "HB:",
                " ",
                "Glicemia:",
                " ",
                "PCT:",
                " ",
            };
            information.Add(rowInformation);
            rowInformation = new List<string>
            {
                "Leucos:",
                " ",
                "DHL:",
                " ",
                "UN:",
                " ",
            };
            information.Add(rowInformation);
            rowInformation = new List<string>
            {
                "Linfos:",
                " ",
                "DD:",
                " ",
                "CREA:",
                " ",
            };
            information.Add(rowInformation);
            rowInformation = new List<string>
            {
                "Troponina:",
                " ",
                "Ferretina:",
                " ",
                "RX Tórax:",
                " ",
            };
            information.Add(rowInformation);
            rowInformation = new List<string>
            {
                "TNT:",
                " ",
                "PCR:",
                " ",
                "Otro:",
                " ",
            };
            information.Add(rowInformation);

            PdfGrid pdfGrid = CreateTable(rowInformation.Count, information.Count, null, information, 12);

            result = pdfGrid.Draw(result.Page, new PointF(0, result.Bounds.Bottom + paragraphAfterSpacing), format);
        }

        private void AddChestRX(PdfDocument doc)
        {
            PdfStandardFont contentTitleFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 12, PdfFontStyle.Bold);
            DrawText("RX TÓRAX", contentTitleFont, result.Bounds.Bottom + paragraphAfterSpacing);

            List<string> headers = new List<string>
            {
                "Descripción:"
            };
            List<List<string>> information = new List<List<string>>();
            List<string> rowInformation = new List<string>
            {
                " "
            };
            information.Add(rowInformation);

            PdfGrid pdfGrid = CreateTable(rowInformation.Count, information.Count, headers, information, 12);

            result = pdfGrid.Draw(result.Page, new PointF(0, result.Bounds.Bottom + paragraphAfterSpacing), format);
        }

        private void AddArterialGases(PdfDocument doc)
        {
            PdfStandardFont contentTitleFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 12, PdfFontStyle.Bold);
            DrawText("GASES ARTERIALES", contentTitleFont, result.Bounds.Bottom + paragraphAfterSpacing);

            List<string> headers = new List<string>
            {
                "Fecha:",
                " ",
                "Fecha:",
                "",
                "Fecha:",
                ""
            };
            List<List<string>> information = new List<List<string>>();
            List<string> rowInformation = new List<string>
            {
                "PH",
                " ",
                "PH",
                "",
                "PH",
                ""
            };
            information.Add(rowInformation);
            rowInformation = new List<string>
            {
                "PO2",
                " ",
                "PO2",
                "",
                "PO2",
                ""
            };
            information.Add(rowInformation);
            rowInformation = new List<string>
            {
                "PCO2",
                " ",
                "PCO2",
                "",
                "PCO2",
                ""
            };
            information.Add(rowInformation);
            rowInformation = new List<string>
            {
                "HCO3",
                " ",
                "HCO3",
                "",
                "HCO3",
                ""
            };
            information.Add(rowInformation);
            rowInformation = new List<string>
            {
                "LAC",
                " ",
                "LAC",
                "",
                "LAC",
                ""
            };
            information.Add(rowInformation);
            rowInformation = new List<string>
            {
                "PAFI",
                " ",
                "PAFI",
                "",
                "PAFI",
                ""
            };
            information.Add(rowInformation);
            rowInformation = new List<string>
            {
                "FIO2",
                " ",
                "FIO2",
                "",
                "FIO2",
                ""
            };
            information.Add(rowInformation);
            PdfGrid pdfGrid = CreateTable(rowInformation.Count, information.Count, headers, information, 12);

            result = pdfGrid.Draw(result.Page, new PointF(0, result.Bounds.Bottom + paragraphAfterSpacing), format);

            information = new List<List<string>>();
            rowInformation = new List<string>
            {
                "Invasiones (Vía Periférica – CVC):",
                " ",
                ""
            };
            information.Add(rowInformation);
            rowInformation = new List<string>
            {
                "Medicamentos:",
                " ",
                ""
            };
            information.Add(rowInformation);
            rowInformation = new List<string>
            {
                "Dispositivo de oxigenación:",
                " ",
                ""
            };
            information.Add(rowInformation);
            rowInformation = new List<string>
            {
                "Estratificación del paciente ",
                "Índice respiratorio - PaO2/FiO2:",
                " "
            };
            information.Add(rowInformation);
            rowInformation = new List<string>
            {
                "",
                "Escala SOFA (Ver anexo 1):",
                " "
            };
            information.Add(rowInformation);
            rowInformation = new List<string>
            {
                "",
                "Factores de riesgo identificables al ingreso\n(Obesidad, asma, arritmia, etc.):",
                " "
            };
            information.Add(rowInformation);
            rowInformation = new List<string>
            {
                "",
                "ETA:",
                " "
            };
            information.Add(rowInformation);

            pdfGrid = CreateTable(rowInformation.Count, information.Count, null, information, 12);

            for (int index = 0; index < 3; index++)
            {
                pdfGrid.Rows[index].Cells[1].ColumnSpan = 2;
            }
            pdfGrid.Rows[3].Cells[0].RowSpan = 4;

            result = pdfGrid.Draw(result.Page, new PointF(0, result.Bounds.Bottom + paragraphAfterSpacing), format);
        }

        private void AddAnalisis(PdfDocument doc)
        {
            PdfStandardFont contentTitleFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 12, PdfFontStyle.Bold);
            DrawText("ANÁLISIS", contentTitleFont, result.Bounds.Bottom + paragraphAfterSpacing);

            List<string> headers = new List<string>
            {
                "Análisis"
            };
            List<List<string>> information = new List<List<string>>();
            List<string> rowInformation = new List<string>
            {
                " "
            };
            information.Add(rowInformation);

            PdfGrid pdfGrid = CreateTable(rowInformation.Count, information.Count, headers, information, 12);

            result = pdfGrid.Draw(result.Page, new PointF(0, result.Bounds.Bottom + paragraphAfterSpacing), format);
        }

        private void AddAnnex1(PdfDocument doc)
        {
            PdfStandardFont contentTitleFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 12, PdfFontStyle.Bold);
            DrawText("Escala SOFA (Sepsis related Organ Failure Assessment)", contentTitleFont, result.Bounds.Bottom + paragraphAfterSpacing);

            List<string> headers = new List<string>
            {
                "Criterio",
                "0",
                "+1",
                "+2",
                "+3",
                "+4",
                "PTS."
            };
            List<List<string>> information = new List<List<string>>();
            List<string> rowInformation = new List<string>
            {
                "Respiración\nPaO2/FIO2 (mm Hg)",
                " ",
                " ",
                " ",
                " ",
                " ",
                " "
            };
            information.Add(rowInformation);
            rowInformation = new List<string>
            {
                "Coagulación\nPlaquetas 103/mm3",
                " ",
                " ",
                " ",
                " ",
                " ",
                " "
            };
            information.Add(rowInformation);
            rowInformation = new List<string>
            {
                "Hepático\nBilirubina (mg/dL)",
                " ",
                " ",
                " ",
                " ",
                " ",
                " "
            };
            information.Add(rowInformation);
            rowInformation = new List<string>
            {
                "Cardiovascular\nTensión arterial",
                " ",
                " ",
                " ",
                " ",
                " ",
                " "
            };
            information.Add(rowInformation);
            rowInformation = new List<string>
            {
                "SNC\nEscala de Glasgow",
                " ",
                " ",
                " ",
                " ",
                " ",
                " "
            };
            information.Add(rowInformation);
            rowInformation = new List<string>
            {
                "Renal\nCreatinina (mg/dL) o\nFlujo urinario (mL/d)",
                " ",
                " ",
                " ",
                " ",
                " ",
                " "
            };
            information.Add(rowInformation);

            PdfGrid pdfGrid = CreateTable(rowInformation.Count, information.Count, headers, information, 12);

            pdfGrid.Columns[6].Width = 40;

            result = pdfGrid.Draw(result.Page, new PointF(0, result.Bounds.Bottom + paragraphAfterSpacing), format);
        }

        private void AddIncidentMultimedia(PdfDocument doc, PdfModel information)
        {
            PdfStandardFont contentTitleFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 12, PdfFontStyle.Bold);
            DrawText("CONTENIDO MULTIMEDIA ASOCIADO AL INCIDENTE", contentTitleFont, result.Bounds.Bottom + paragraphAfterSpacing);
            if (information.ActionTypes != null && information.ActionTypes.Count != 0)
            {
                List<string> headers = new List<string>
                {
                    "Nombre",
                    "Tipo",
                    "Fecha"
                };
                for (int index = 0; index < information.ActionTypes.Count; index++)
                {
                    PdfTextElement actionName = new PdfTextElement(information.ActionTypes[index].Nombre, contentTitleFont, PdfBrushes.Black);
                    result = actionName.Draw(result.Page, new RectangleF(0, result.Bounds.Bottom + paragraphAfterSpacing, page.GetClientSize().Width, page.GetClientSize().Height), format);

                    if (information.ExistingFiles[index] != null && information.ExistingFiles[index].Count != 0)
                    {
                        List<List<string>> tableInformation = new List<List<string>>();
                        int numberOfRows = 0;

                        for (int counter = 0; counter < information.ExistingFiles[index].Count; counter++)
                        {
                            List<string> rowInformation = new List<string>
                            {
                                information.ExistingFiles[index][counter].Nombre,
                                information.ExistingFiles[index][counter].Tipo,
                                information.ExistingFiles[index][counter].Fecha_Hora.ToString()
                            };
                            tableInformation.Add(rowInformation);
                            numberOfRows++;
                        }
                        PdfGrid pdfGrid = CreateTable(headers.Count, tableInformation.Count, headers, tableInformation, 12);

                        result = pdfGrid.Draw(result.Page, new PointF(0, result.Bounds.Bottom + paragraphAfterSpacing), format);
                    } else
                    {
                        List<List<string>> tableInformation = new List<List<string>>();
                        List<string> rowInformation = new List<string>
                        {
                            "No se adjuntó ninguno archivo multimedia",
                            "",
                            ""
                        };
                        tableInformation.Add(rowInformation);
                        PdfGrid pdfGrid = CreateTable(headers.Count, tableInformation.Count, headers, tableInformation, 12);

                        pdfGrid.Rows[1].Cells[0].ColumnSpan = 3;

                        result = pdfGrid.Draw(result.Page, new PointF(0, result.Bounds.Bottom + paragraphAfterSpacing), format);
                    }
                }
            }
        }
    }
}
