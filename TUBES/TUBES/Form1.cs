using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TUBES
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                // Ambil input berat dan tinggi
                double weight = double.Parse(txtWeight.Text); // Berat dalam kg
                double height = double.Parse(txtHeight.Text); // Tinggi dalam cm

                // Ubah tinggi dari cm ke meter
                height = height / 100;

                // Kalkulasi BMI
                double bmi = weight / (height * height);

                // Tentukan kategori BMI
                string category = "";
                if (bmi < 18.5)
                    category = "Underweight (BMI < 18.5)";
                else if (bmi < 24.9)
                    category = "Normal weight (BMI 18.5 - 24.9)";
                else if (bmi < 29.9)
                    category = "Overweight (BMI 25 - 29.9)";
                else
                    category = "Obesity (BMI ≥ 30)";

                // Tampilkan hasil BMI dan kategori
                lblBMIResult.Text = $"BMI: {bmi:F2}\nCategory: {category}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please enter valid numerical values for weight and height.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtWeight.Clear();
            txtHeight.Clear();
            lblBMIResult.Text = "";
        }

        private void btnCalculateIH_Click(object sender, EventArgs e)
        {
            try
            {
                // Ambil input data
                int week = int.Parse(cmbPregnancyWeek.SelectedItem.ToString().Replace("Week ", ""));
                bool isTwin = rdoYesTwin.Checked; // True jika hamil kembar
                double height = double.Parse(txtHeightIH.Text); // Tinggi badan dalam cm
                double weightBeforePregnancy = double.Parse(txtWeightBeforePregnancy.Text); // Berat sebelum hamil dalam kg
                double weightNow = double.Parse(txtWeightNow.Text); // Berat saat ini dalam kg

                // Perhitungan rentang berat badan yang ideal
                double minGain, maxGain;

                if (isTwin)
                {
                    // Rentang kenaikan berat untuk kehamilan kembar (perkiraan)
                    minGain = 16 + (week - 1) * 0.8; // Nilai awal dan kenaikan mingguan
                    maxGain = 25 + (week - 1) * 1.2;
                }
                else
                {
                    // Rentang kenaikan berat untuk kehamilan tunggal
                    minGain = 11.5 + (week - 1) * 0.5;
                    maxGain = 16 + (week - 1) * 0.7;
                }

                // Berat badan ideal berdasarkan tinggi badan (BMI sederhana)
                double minIdealWeight = height / 100 * height / 100 * 18.5;
                double maxIdealWeight = height / 100 * height / 100 * 24.9;

                // Output hasil
                rtxtWeightResult.Text = $"Pregnancy Week: {week}\n" +
                                        $"Twin Pregnancy: {(isTwin ? "Yes" : "No")}\n" +
                                        $"Ideal Weight Gain (kg): {minGain:F1} - {maxGain:F1}\n" +
                                        $"Your Pre-Pregnancy Weight: {weightBeforePregnancy} kg\n" +
                                        $"Your Current Weight: {weightNow} kg\n\n" +
                                        $"Healthy Weight Range Before Pregnancy (Based on BMI): {minIdealWeight:F1} - {maxIdealWeight:F1} kg\n" +
                                        $"-----------------------------------------------\n" +
                                        $"{(weightNow >= weightBeforePregnancy + minGain && weightNow <= weightBeforePregnancy + maxGain ? "Your weight gain is within the ideal range." : "Your weight gain is outside the ideal range.")}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please ensure all fields are filled in correctly.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClearIH_Click(object sender, EventArgs e)
        {
            // Bersihkan semua input dan hasil
            cmbPregnancyWeek.SelectedIndex = -1;
            rdoYesTwin.Checked = false;
            rdoNoTwin.Checked = true;
            txtHeightIH.Clear();
            txtWeightBeforePregnancy.Clear();
            txtWeightNow.Clear();
            rtxtWeightResult.Clear();
        }

        private void btnCalculateMO_Click(object sender, EventArgs e)
        {
            try
            {
                // Ambil input dari user
                DateTime lastPeriodDate = dtpLastPeriod.Value; // Tanggal periode terakhir
                string cycleText = cmbCycleLength.SelectedItem.ToString(); // Ambil nilai dari ComboBox
                int cycleLength = int.Parse(cycleText.Split(' ')[0]); // Ekstrak angka sebelum kata "days"

                // Hitung tanggal ovulasi
                DateTime ovulationDate = lastPeriodDate.AddDays(cycleLength / 2);
                DateTime fertileStart = ovulationDate.AddDays(-3); // Awal masa subur
                DateTime fertileEnd = ovulationDate.AddDays(3); // Akhir masa subur

                // Tampilkan hasil di RichTextBox
                rtxtResultMO.Text = $"Ovulation Date: {ovulationDate:MMMM dd, yyyy}\n" +
                                  $"Fertile Window: {fertileStart:MMMM dd, yyyy} - {fertileEnd:MMMM dd, yyyy}\n\n" +
                                  "Note: This is an estimation. Consult your doctor for more accurate results.";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please ensure all fields are filled in correctly.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClearMO_Click(object sender, EventArgs e)
        {
            // Reset semua input dan hasil
            dtpLastPeriod.Value = DateTime.Now;
            cmbCycleLength.SelectedItem = "28 days"; // Set default ke nilai "28"
            rtxtResultMO.Clear();
        }

        private void btnCalculateUK_Click(object sender, EventArgs e)
        {
            try
            {
                // Ambil input dari user
                DateTime lastPeriod = dtpLastPeriodUK.Value; // Tanggal periode terakhir
                string cycleText = cmbCycleLength.SelectedItem.ToString(); // Ambil nilai dari ComboBox
                int cycleLength = int.Parse(cycleText.Split(' ')[0]); // Panjang siklus rata-rata

                // Header hasil
                string result = "Week\tDate Range\t\tTrimester\tMilestones\n";

                // Iterasi tiap minggu hingga minggu ke-42
                for (int week = 1; week <= 42; week++)
                {
                    DateTime weekStart = lastPeriod.AddDays((week - 1) * 7 + (cycleLength - 28));
                    DateTime weekEnd = weekStart.AddDays(6);

                    string trimester = GetTrimester(week); // Tentukan trimester
                    string milestone = GetMilestone(week); // Tentukan milestone

                    result += $"Week {week}\t{weekStart:MMM dd, yyyy} - {weekEnd:MMM dd, yyyy}\t{trimester}\t{milestone}\n";
                }

                // Tampilkan hasil di RichTextBox
                rtxtResultUK.Text = result;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while calculating pregnancy weeks. Please ensure all fields are filled in correctly.",
                    "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetTrimester(int week)
        {
            if (week >= 1 && week <= 12) return "First Trimester";
            if (week >= 13 && week <= 27) return "Second Trimester";
            return "Third Trimester";
        }

        private string GetMilestone(int week)
        {
            if (week == 3) return "Baby conceived";
            else if (week == 4) return "Pregnancy test positive";
            else if (week == 6) return "Heartbeat detectable by ultrasound";
            else if (week == 13) return "Miscarriage risk decreases";
            else if (week == 18) return "Baby begins making movements, can hear sounds, gender identifiable";
            else if (week == 23) return "Premature baby may survive";
            else if (week == 28) return "Baby can breathe";
            else if (week == 38) return "Full Term";
            else return "";
        }

        private void btnClearUK_Click(object sender, EventArgs e)
        {
            dtpLastPeriodUK.Value = DateTime.Now;
            cmbCycleLengthUK.SelectedItem = "28 days"; // Set default ke 28 hari
            rtxtResultUK.Clear();
        }
    }
}
