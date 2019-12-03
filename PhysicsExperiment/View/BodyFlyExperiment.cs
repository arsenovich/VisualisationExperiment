using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PhysicsExperiment.Physics.Experiments;
using PhysicsExperiment.Physics.Models;

namespace PhysicsExperiment.View
{
    public partial class BodyFlyExperiment : Form
    {
        // делегат - ссылка на  метод постройки эксперимента
        private Action builder;

        // построенный эксперимент
        private IBodyFlightExperiment experiment;

        // Конструктор - задает начальные параметры
        public BodyFlyExperiment()
        {
            // строит грайический интерфейс
            InitializeComponent();

            // задает эксперимент по умолчанию
            builder = () => buildSimpleExperiment();
        }

        // рисует график
        private void drawExperiment()
        {
            // размеры области для рисования
            int w = pictureBoxVisualisation.Width;
            int h = pictureBoxVisualisation.Height;
            
            // маштабирование
            double scaleY = 0.9f * w / experiment.MaxY;
            double scaleX = 0.9f * h / experiment.MaxX;

            // число линий графика по ширине
            int countW = 25;
            // число линий графика по высоте
            int countH = 25;

            Bitmap bmp = new Bitmap(w, h);

            using (Graphics e = Graphics.FromImage(bmp))
            {
                e.Clear(Color.White);
                float dw = 1f * w / countW;
                float dh = 1f * h / countH;
                Pen cellsPen = new Pen(Color.Gray, 2f);
                
                // рисуем вертикальные линии
                for (int i = 0; i < countH; ++i) {
                    Point start = new Point((int)(i * dw), 0);
                    Point end = new Point((int)(i * dw), h);
                    e.DrawLine(cellsPen, start, end);
                }
                
                // рисуем горизонтальные линнии
                for (int i = 0; i < countH; ++i)
                {
                    Point start = new Point(0, (int)(i * dh));
                    Point end = new Point(w, (int)(i * dh));
                    e.DrawLine(cellsPen, start, end);
                }

                Entity entity = experiment.Entity;

                // позиция тела с маштабированием
                float px = (float) (scaleX * entity.Position.X);
                float py = 1f * h - (float) (scaleY * entity.Position.Y);

                RectangleF rect = new RectangleF(new PointF(px - 2f, py - 2f), new SizeF(4f, 4f));
                e.DrawEllipse(new Pen(Color.Red, 1), rect);

                pictureBoxVisualisation.Image = bmp;
            }
        }

        // метод постройки эксперимента с сопротивлением
        private void buildHardExperiment()
        {
            try
            {
                // получаем значения
                double mass = Convert.ToDouble(textBoxMass.Text);
                double pX = Convert.ToDouble(textBoxPositionX.Text);
                double pY = Convert.ToDouble(textBoxPositionY.Text);
                double vX = Convert.ToDouble(textBoxVX.Text);
                double vY = Convert.ToDouble(textBoxVY.Text);

                double R = Convert.ToDouble(textBoxR.Text);

                int timerTick = Convert.ToInt32(textBoxDT.Text);

                Entity entity = new Entity(mass,
                    new Vector(pX, pY),
                    new Vector(vX, vY));

                experiment = new HardBodyFlyExperiment(entity, R);

                timerExperiment.Interval = timerTick;
                timerExperiment.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exeption", ex.Message);
            }
        }

        // метод постройки сложного эксперимена
        private void buildSimpleExperiment()
        {
            try
            {
                // получаем значения
                double mass = Convert.ToDouble(textBoxMass.Text);
                double pX = Convert.ToDouble(textBoxPositionX.Text);
                double pY = Convert.ToDouble(textBoxPositionY.Text);
                double vX = Convert.ToDouble(textBoxVX.Text);
                double vY = Convert.ToDouble(textBoxVY.Text);

                int timerTick = Convert.ToInt32(textBoxDT.Text); 

                Entity entity = new Entity(mass,
                    new Vector(pX, pY),
                    new Vector(vX, vY));

                experiment = new SimleBodyFlightExperiment(entity);

                timerExperiment.Interval = timerTick;
                timerExperiment.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exeption", ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // строим эксперимент
            builder();
        }

        private void timerExperiment_Tick(object sender, EventArgs e)
        {
            drawExperiment();
            
            if (experiment.Entity.Position.Y < 0.0)
                timerExperiment.Stop();

            richTextStatistic.Text = experiment.Text;

            experiment.Tick(1.0 * timerExperiment.Interval / 1000);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            // задаем что эксперимент без сопротивления
            builder = () => buildSimpleExperiment();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            // задаем что эксперимент с сопротивлением
            builder = () => buildHardExperiment();
        }
    }
}
