    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

namespace PBIL {


    public interface IEvaluation {
        double Evaluate( int[] chromosome );
    }
    public class TFitnessFunction : IEvaluation {
        int tValue;
        public TFitnessFunction( int tValue ) {
            this.tValue=tValue;
        }
        public double Evaluate( int[] chromosome ) {
            int startingOnes=0;
            int index=0;
            while(index<chromosome.Length && chromosome[index] == 1) {
                startingOnes++;
                index++;
            }
            int endingZeros=0;
            index=chromosome.Length-1;
            while(index >= 0 && chromosome[index] == 0) {
                endingZeros++;
                index--;
            }
            double rs=0;
            if(startingOnes> tValue && endingZeros>tValue)
                rs += 100+ tValue;
            rs += (startingOnes>endingZeros) ? startingOnes : endingZeros;
            return rs;
        }
    }
    public class Sample {
        public double fitness;
        public int []chromosome;
        public Sample( double fitness, int[] chromosome ) {
            this.fitness = fitness;
            this.chromosome = chromosome;
        }
    }
    public class PBILAlgorithm {
        /// PBIL hyper parameter
        double learningRate;
        int numberOfSamples;
        int numOfLearningVectors;
        // problem parameters 
        IEvaluation evaluator;
        int chromosomeLength;

        //PBIL internal probability vector
        double []probabilityVector;

       

        //
        Random rd;
        public PBILAlgorithm( int numberOfSamples, double learningRate, int numOfLearningVectors, IEvaluation evaluator, int chromosomeLength ) {
            this.evaluator = evaluator;
            this.chromosomeLength = chromosomeLength;
            this.numberOfSamples = numberOfSamples;
            this.learningRate = learningRate;
            this.numOfLearningVectors=numOfLearningVectors;
          
            rd = new Random();
            probabilityVector = new double[chromosomeLength];
        }
        public void Initialize() {
            for(int i = 0; i<chromosomeLength; i++)
                probabilityVector[i] = .5D;
        }
        public int[] Generate_Sample_Vector_According_To_Probabilities() {
            int []chromosome = new int[chromosomeLength];
            for(int i = 0; i<chromosomeLength; i++)
                chromosome[i] = (rd.NextDouble()<=probabilityVector[i]) ? 1 : 0;
            return chromosome;
        }
        public void RunCycle() {
            //******Generate Examples*********
            Sample []samples = new Sample[numberOfSamples];
            for(int i = 0; i<numberOfSamples; i++) {
                int []chromosome = Generate_Sample_Vector_According_To_Probabilities();
                double fitness = evaluator.Evaluate(chromosome);
                samples[i] = new Sample( fitness, chromosome );
            }
            //******Sort samples from best to worst acourding to fitness evaluation***************
            Array.Sort<Sample>( samples, ( Sample a, Sample b ) => { return b.fitness.CompareTo( a.fitness ); } );

            //********Update probability vectors towards best solutions*********
            for(int j = 0; j<numOfLearningVectors; j++) {
                for(int i = 0; i<chromosomeLength; i++)
                    probabilityVector[i] = probabilityVector[i] * (1-learningRate) + samples[j].chromosome[i] * learningRate;
            }
           
        }
        public void RunToEnd( int cycles ) {
            Initialize();
            for(int i = 1; i<cycles; i++) {
                RunCycle();
            }
        }
        public double[][] RunToEndWithRenturn( int cycles ) {
            double [][]probabilityVectors = new double[cycles][];
            Initialize();
            probabilityVectors[0] = (double[]) probabilityVector.Clone();
            for(int i = 1; i<cycles; i++) {
                RunCycle();
                probabilityVectors[i] = (double[]) probabilityVector.Clone();
            }
            return probabilityVectors;
        }

        //not important
        public double[] ProbabilityVector {
            get { return (double[]) probabilityVector.Clone(); }
        }
        public Sample[] GenerateSamples() {
            Sample []samples = new Sample[numberOfSamples];
            for(int i = 0; i<numberOfSamples; i++) {
                int []chromosome = Generate_Sample_Vector_According_To_Probabilities();
                double fitness = evaluator.Evaluate(chromosome);
                samples[i] = new Sample( fitness, chromosome );
            }
            return samples;
        }
    }
}

