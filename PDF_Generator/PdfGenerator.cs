using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Hotel_Bokking_System.DTO;
using System.IO;
using System.Text;

namespace Hotel_Bokking_System.Services
{
    public static class PdfGenerator
    {
        private static readonly byte[] PDF_SIGNATURE = { 0x25, 0x50, 0x44, 0x46 }; // %PDF

        public static byte[] GenerateBill(Bill_DTO bill)
        {
            try
            {
                var document = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(2, Unit.Centimetre);
                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(x => x.FontSize(14).FontFamily("Arial"));

                        // ===== Header =====
                        page.Header().Column(headerCol =>
                        {
                            headerCol.Item().Row(row =>
                            {
                                row.RelativeItem().Column(col =>
                                {
                                    col.Item().Text("HOTEL BOOKING SYSTEM")
                                        .Bold().FontSize(24).FontColor(Colors.Blue.Darken2);
                                    col.Item().Text("Professional Invoice")
                                        .FontSize(12).FontColor(Colors.Grey.Darken1);
                                });
                                row.ConstantItem(120).AlignRight().Text($"Invoice #{bill.BookingID}")
                                    .Bold().FontSize(16).FontColor(Colors.Blue.Medium);
                            });
                            headerCol.Item().PaddingTop(10).LineHorizontal(2).LineColor(Colors.Blue.Medium);
                        });

                        // ===== Content =====
                        page.Content().PaddingTop(20).Column(col =>
                        {
                            col.Spacing(15);

                            // --- Customer Information ---
                            col.Item().Background(Colors.Grey.Lighten4).Padding(10).Column(custCol =>
                            {
                                custCol.Item().Text("CUSTOMER INFORMATION").Bold().FontSize(16).FontColor(Colors.Blue.Darken1);
                                custCol.Item().PaddingTop(5).Text($"Name: {bill.CustomrName}").FontSize(12);
                                custCol.Item().Text($"Booking Reference: {bill.BookingID}").FontSize(12);
                            });

                            // --- Booking Details ---
                            col.Item().Background(Colors.Blue.Lighten5).Padding(10).Column(bookingCol =>
                            {
                                bookingCol.Item().Text("BOOKING DETAILS").Bold().FontSize(16).FontColor(Colors.Blue.Darken1);
                                bookingCol.Item().PaddingTop(5).Row(detailRow =>
                                {
                                    var nights = Math.Max((bill.CheckOut - bill.CheckIn).Days, 1);

                                    detailRow.RelativeItem().Column(leftCol =>
                                    {
                                        leftCol.Item().Text($"Room Number: {bill.RoomNumber}").FontSize(12);
                                        leftCol.Item().Text($"Check-In: {bill.CheckIn:dddd, MMMM dd, yyyy}").FontSize(12);
                                        leftCol.Item().Text($"Check-Out: {bill.CheckOut:dddd, MMMM dd, yyyy}").FontSize(12);
                                    });

                                    detailRow.RelativeItem().Column(rightCol =>
                                    {
                                        rightCol.Item().Text($"Duration: {nights} night(s)").FontSize(12);
                                        rightCol.Item().Text($"Rate per night: {bill.Amount:C}").FontSize(12);
                                    });
                                });
                            });

                            // --- Payment Summary ---
                            col.Item().Background(Colors.Green.Lighten5).Padding(10).Column(payCol =>
                            {
                                var nights = Math.Max((bill.CheckOut - bill.CheckIn).Days, 1);
                                var total = bill.Amount * nights;

                                payCol.Item().Text("PAYMENT SUMMARY").Bold().FontSize(16).FontColor(Colors.Green.Darken2);
                                payCol.Item().PaddingTop(5).Row(payRow =>
                                {
                                    payRow.RelativeItem().Text($"Total Amount: {total:C}").Bold().FontSize(14);
                                    payRow.ConstantItem(150).AlignRight().Text($"Status: {bill.paymentStatus}")
                                        .Bold().FontSize(12)
                                        .FontColor(bill.paymentStatus.ToString().ToUpper() == "PAID"
                                            ? Colors.Green.Darken1
                                            : Colors.Red.Darken1);
                                });
                            });

                            // --- Invoice Information ---
                            col.Item().PaddingTop(20).Text($"Invoice Generated: {bill.Created:dddd, MMMM dd, yyyy 'at' HH:mm}")
                                .FontSize(10).FontColor(Colors.Grey.Darken1);
                        });

                        // ===== Footer =====
                        page.Footer().Column(footerCol =>
                        {
                            footerCol.Item().LineHorizontal(1).LineColor(Colors.Grey.Medium);
                            footerCol.Item().PaddingTop(10).Row(footerRow =>
                            {
                                footerRow.RelativeItem().Text("Thank you for choosing our hotel!")
                                    .FontSize(10).FontColor(Colors.Grey.Darken1);
                                footerRow.ConstantItem(200).AlignRight().Text($"© {DateTime.Now.Year} Hotel Booking System")
                                    .FontSize(10).FontColor(Colors.Grey.Darken1);
                            });
                        });
                    });
                });

                var pdfBytes = document.GeneratePdf();

                if (!IsPdfValid(pdfBytes))
                    throw new InvalidOperationException("Generated PDF is corrupted or invalid");

                return pdfBytes;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PDF generation failed: {ex.Message}");
                throw;
            }
        }

        // ===== PDF Validation =====
        public static bool IsPdfValid(byte[] fileBytes)
        {
            if (fileBytes == null || fileBytes.Length < 8)
                return false;

            for (int i = 0; i < PDF_SIGNATURE.Length; i++)
            {
                if (fileBytes[i] != PDF_SIGNATURE[i])
                    return false;
            }

            string header = Encoding.ASCII.GetString(fileBytes, 0, Math.Min(8, fileBytes.Length));
            return header.StartsWith("%PDF-");
        }
    }
}
