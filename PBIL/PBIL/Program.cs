using System;
using System.Diagnostics;
using System.IO;

namespace PBIL {
    class Program {



        static void Plot( double[][] probabilityVectors, string gnuPlotDir, string outpuDir ) {
            //string fineName = info.Dir+info.FileOutName;
            //WriteDataFiles( report, info );
            //WritePlt( info );
            Process plotProcess = new Process();
            plotProcess.StartInfo.FileName = gnuPlotDir+"gnuplot.exe";
            plotProcess.StartInfo.UseShellExecute = false;
            plotProcess.StartInfo.RedirectStandardInput = true;
            plotProcess.Start();
            StreamWriter sw = plotProcess.StandardInput;
                        sw.WriteLine( "$data << EOD" );
                      for(int n = 0; n<probabilityVectors[0].Length; n++) {
                        sw.Write( "{0}", n+1 );
                      for(int i = 0; i<probabilityVectors.Length; i++) {
                        sw.Write( " {0}", probabilityVectors[i][n] );
              }
                sw.WriteLine();
                      }
                    sw.WriteLine( "EOD" );
            //            sw.WriteLine( "set terminal png" );
            //           sw.WriteLine( "set output '{0}PBIL.png'", outpuDir );
            //           sw.WriteLine( "set xlabel \"bit\"" );
            //          sw.WriteLine( "set autoscale" );
            //        sw.WriteLine( "set ylabel \"Probability\"" );
            //      sw.WriteLine( "set title \"Probability of Each Bit");
            //    sw.WriteLine( "set key off" );
            //  sw.WriteLine( "set style data line" );
            // sw.WriteLine( "plot \"$data\" using 1:{0} title \"Prob. Vector\" lw 4, \\", probabilityVectors.Length );
            //   sw.Flush();

            //        sw.WriteLine( "reset" );
            //        sw.WriteLine( "set term gif animate" );
            //        sw.WriteLine( "set output '{0}PBIL.gif'", outpuDir );
            //        sw.WriteLine( "n=24    #n frames" );
            //        sw.WriteLine( "set xlabel \"bit\"" );
            //        sw.WriteLine( "set autoscale" );
            //        sw.WriteLine( "set ylabel \"Probability\"" );
            //        sw.WriteLine( "set title \"Probability of Each Bit" );
            //        sw.WriteLine( "set key off" );
            //        sw.WriteLine( "set style data line" );
            //        sw.WriteLine( "plot \"$data\" using 1:2 title \"Prob. Vector\" lw 4, \\" );
            ////        for(int i=1;i<probabilityVectors.Length;i++)
            //  //          sw.WriteLine( "replot \"$data\" using 1:{0} title \"Prob. Vector\" lw 4, \\", i+2 );
            //        sw.WriteLine( "set output" );
            //            Console.ReadLine();

            sw.WriteLine( "reset" );
            sw.WriteLine( "set term gif animate" );
            sw.WriteLine( "set output \'{0}PBIL-animate.gif\'", outpuDir );
            sw.WriteLine( "n=60    #n frames" );
            sw.WriteLine( "set key off" );
            sw.WriteLine( "set xrange [1:100]" );
            sw.WriteLine( "set yrange [0:1]" );
                    sw.WriteLine( "set xlabel \"bit\"" );
                    sw.WriteLine( "set ylabel \"Probability\"" );
                    sw.WriteLine( "set title \"Probability of Each Bit" );

            for(int i = 0; i<probabilityVectors.Length; i+=2) {
                sw.WriteLine( "set title \"Probability of Each Bit, Gen: {0}", i+1 );
                //sw.WriteLine( "set label 1 center \"Gen: {0}\" ", i+1 );
                sw.WriteLine( "plot \"$data\" using 1:{0} w l lt 1 lw 5 title \"Prob\"", i+2 );
                //sw.WriteLine( "unset label 1" );
            }
            sw.WriteLine( "set output" );

            sw.Flush();
            sw.Close();
            plotProcess.Close();

        }
        static void Print3D( string gnuPlotDir, string outpuDir ) {
            Process plotProcess = new Process();
            plotProcess.StartInfo.FileName = gnuPlotDir+"gnuplot.exe";
            plotProcess.StartInfo.UseShellExecute = false;
            plotProcess.StartInfo.RedirectStandardInput = true;
            plotProcess.Start();
            StreamWriter sw = plotProcess.StandardInput;
            sw.WriteLine( "$data << EOD" );
            int tValue=20;
            for(int startingOnes = 0; startingOnes<=100; startingOnes+=1) {
                for(int endingZeros = 0; endingZeros<=100; endingZeros+=1) {
                    if(startingOnes+endingZeros<=100) {
                        int rs=0;
                        if(startingOnes> tValue && endingZeros>tValue)
                            rs += 100+ tValue;
                        rs += (startingOnes>endingZeros) ? startingOnes : endingZeros;
                        sw.WriteLine( "{0} {1} {2}", startingOnes, endingZeros, rs );
                    }
                }
            }
            sw.WriteLine( "EOD" );
            sw.WriteLine( "set terminal png" );
            sw.WriteLine( "set output '{0}Function.png'", outpuDir );
            sw.WriteLine( "set xlabel \"# of ones\"" );
                      sw.WriteLine( "set autoscale" );
                    sw.WriteLine( "set ylabel \"# of zeros\"" );
//                  sw.WriteLine( "set title \"Probability of Each Bit");
                sw.WriteLine( "set key off" );
           //   sw.WriteLine( "set style data points" );
            sw.WriteLine( "set view 50,310" );
            //sw.WriteLine( "set dgrid3d" );
           // sw.WriteLine( "set contour");
             sw.WriteLine( "splot \"$data\" with dots " );
               sw.Flush();

            
            sw.WriteLine( "reset" );
            sw.WriteLine( "set term gif animate" );
            sw.WriteLine( "set output \'{0}Function-animate.gif\'", outpuDir );
            sw.WriteLine( "n=60    #n frames" );
            sw.WriteLine( "set key off" );
            sw.WriteLine( "set xlabel \"# of ones\"" );
            sw.WriteLine( "set autoscale" );
            sw.WriteLine( "set ylabel \"# of zeros\"" );

            for(int i = 0; i<150; i++) {
                int r = 260+i;
                r = (r%360);
                sw.WriteLine( "set view 50,{0}", r );
                sw.WriteLine( "splot \"$data\" with dots " );
            }
            sw.WriteLine( "set output" );
            sw.Flush();

            sw.WriteLine( "reset" );
            sw.WriteLine( "set term gif animate" );
            sw.WriteLine( "set output \'{0}Function-animate-2.gif\'", outpuDir );
            sw.WriteLine( "n=60    #n frames" );
            sw.WriteLine( "set key off" );
            sw.WriteLine( "set xlabel \"# of ones\"" );
            sw.WriteLine( "set autoscale" );
            sw.WriteLine( "set ylabel \"# of zeros\"" );

            for(int i = 0; i<150; i++) {
                sw.WriteLine( "set view {0},20", i );
                sw.WriteLine( "splot \"$data\" with dots " );
            }
            sw.WriteLine( "set output" );
            sw.Flush();

            sw.Close();
            plotProcess.Close();

        }
        static void Main( string[] args ) {
            int populationSize=200;
            double learningRate = .01;
            int numberOfLearningVectors = 2;
            string gnuPlotDir= @"C:\Program Files (x86)\gnuplot\bin\";
            string outpuDir = @"C:\Users\ARegin\Desktop\PBIL_DATA\";
            int chromosomeLength = 100;
            int tValue=20;
            int numCycles = 1500;
            for(int i = 0; i<args.Length; i++) {
                switch(args[i]) {
                    case "-nc":
                        numCycles = int.Parse( args[i+1] );
                        i++;
                        break;
                    case "-tv":
                        tValue = int.Parse( args[i+1] );
                        i++;
                        break;
                    case "-cl":
                        chromosomeLength = int.Parse( args[i+1] );
                        i++;
                        break;
                    case "-ps":
                        populationSize = int.Parse( args[i+1] );
                        i++;
                        break;
                    case "-lr":
                        learningRate = double.Parse( args[i+1] );
                        i++;
                        break;
                    case "-nlv":
                        numberOfLearningVectors = int.Parse( args[i+1] );
                        i++;
                        break;
                    case "-gd":
                        gnuPlotDir =  args[i+1];
                        i++;
                        break;
                    case "-od":
                        outpuDir = args[i+1];
                        i++;
                        break;
                    default:
                        break;
                }
            }
            PBILAlgorithm pbil = new PBILAlgorithm(populationSize,learningRate,numberOfLearningVectors,new TFitnessFunction(tValue),chromosomeLength);
            double [][]probabilityVectors = pbil.RunToEndWithRenturn(numCycles);
            Plot( probabilityVectors, gnuPlotDir, outpuDir );
            //Console.WriteLine( "End..." );
            //Print3D( gnuPlotDir, outpuDir );
            Console.ReadLine();
        }
    }
}
