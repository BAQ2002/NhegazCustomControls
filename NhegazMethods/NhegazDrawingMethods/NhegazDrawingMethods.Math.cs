using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace NhegazCustomControls
{
    public static partial class NhegazDrawingMethods
    {

        /// <summary>
        /// Transforma um float em .5 ou .0 mais proximo de seu valor original.
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        static float RoundFloat(float valor)
        {
            float parteDecimal = valor - (int)valor;

            // Verifica se a parte decimal é exatamente 0.5
            if (Math.Abs(parteDecimal - 0.5f) < 0.00001f)
            {
                return valor; // mantém com .5
            }
            else
            {
                return (float)Math.Round(valor); // arredonda normalmente
            }
        }

        /// <summary>
        /// Gera uma List de PointF que representa um arco de 90 graus.
        /// </summary>
        public static List<PointF> GenerateArc(int radius)   
        {
            int arcLenght = (int)(Math.PI * radius / 2);          //Comprimento do arco "π*radius/2"
            int segments = Math.Max(1, arcLenght);                //Maior valor entre 1 e arcLenght

            List<PointF> points = [];                             //lista que armazena os pontos do arco

            for (int i = 0; i < segments; i++)                    // * segments
            {
                float t = i / (float)segments;                    //Valor do progresso atual até o final do arco(de 0 a 1)
                float angle = (float)(Math.PI / 2 * t);           //Valor do angulo do progresso atual(de 0° até 90°)

                float fx = radius * (1 - (float)Math.Cos(angle)); //Gera o X do ponto atual
                float fy = radius * (1 - (float)Math.Sin(angle)); //Gera o Y do ponto atual

                var x = RoundFloat(fx);                           //Arrendonda a coordenada X para .0 ou .5
                var y = RoundFloat(fy);                           //Arrendonda a coordenada Y para .0 ou .5

                points.Add(new PointF(x, y));                     //Adiciona o ponto a lista de pontos
            }
            return points;
        }

        /// <summary>
        /// Gera uma List de PointF que representa um arco de 90 graus.
        /// </summary>
        public static List<PointF> GenerateInnerArc(float radius, int arcWidth)
        { 
            int arcLenght = (int)(Math.PI * (radius - (arcWidth -1)) / 2); //Ccomprimento do arco "π*radius - (arcWidth - 1) / 2"
            int segments = Math.Max(1, arcLenght);                         //Maior valor entre 1 e arcLenght

            List<PointF> points = [];                                      //lista que armazena os pontos do arco

            for (int i = 0; i < segments; i++)                             // * segments
            {
                float t = i / (float)segments;                             //Valor do progresso atual até o final do arco(de 0 a 1)
                float angle = (float)(Math.PI / 2 * t);                    //Valor do angulo do progresso atual(de 0° até 90°)

                float fx = radius * (1 - (float)Math.Cos(angle));          //Gera o X do ponto atual
                float fy = radius * (1 - (float)Math.Sin(angle));          //Gera o Y do ponto atual

                var x = RoundFloat(fx);                                    //Arrendonda a coordenada X para .0 ou .5
                var y = RoundFloat(fy);                                    //Arrendonda a coordenada Y para .0 ou .5

                points.Add(new PointF(x, y));                              //Adiciona o ponto a lista de pontos
            }
            return points;
        }
    }
}