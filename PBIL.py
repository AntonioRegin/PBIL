import numpy as np
import random
import matplotlib.pyplot as plt

class TFitnessFunction:
        def __init__(self, tValue):
            self.tValue=tValue

        def Evaluate(self, chromosome ):
            startingOnes=0
            for index in range(chromosome.size):
                if chromosome[index] == 1:
                    startingOnes+=1
                else:
                    break

            endingZeros=0
            for index in range(chromosome.size-1,-1,-1):
                if chromosome[index] == 0:
                    endingZeros+=1
                else:
                    break

            rs=0.0
            if (startingOnes > self.tValue) and (endingZeros>self.tValue):
                rs += 100+ self.tValue
            rs += startingOnes if (startingOnes>endingZeros) else endingZeros
            return rs


class Sample:
        def __init__(self, fitness, chromosome ):
            self.fitness = fitness
            self.chromosome = chromosome


def key(sample):
    return sample.fitness


class PBILAlgorithm:

        # PBIL hyper parameter
        # learningRate;
        # numberOfSamples;
        # numOfLearningVectors;

        # problem parameters
        # evaluator;
        # chromosomeLength;

        # PBIL internal probability vector
        # double []probabilityVector;

        def __init__(self, evaluator ,numberOfSamples=200, learningRate=0.005, numOfLearningVectors=2, chromosomeLength=100):
            self.evaluator = evaluator
            self.numberOfSamples = numberOfSamples;
            self.learningRate = learningRate;
            self.numOfLearningVectors=numOfLearningVectors;
            self.chromosomeLength = chromosomeLength;
            self.probabilityVector = np.zeros(chromosomeLength , dtype=np.float64 )

        def Initialize(self):
            for i in range(self.chromosomeLength):
                self.probabilityVector[i] = 0.5;

        def Generate_Sample_Vector_According_To_Probabilities(self):
            chromosome = np.zeros(self.chromosomeLength,dtype=np.int32);
            for i in range(self.chromosomeLength):
                chromosome[i] =  1 if(random.random() <= self.probabilityVector[i]) else 0
            return chromosome

        def GenerateSamples(self):
            samples = []
            for i in range(self.numberOfSamples):
                chromosome = self.Generate_Sample_Vector_According_To_Probabilities()
                fitness = self.evaluator.Evaluate(chromosome)
                samples[i].add(Sample( fitness, chromosome ))
            return samples

        def RunCycle(self):
            #******Generate Examples*********
            samples = []
            for i in range(self.numberOfSamples):
                chromosome = self.Generate_Sample_Vector_According_To_Probabilities()
                fitness = self.evaluator.Evaluate(chromosome)
                samples.append(Sample( fitness, chromosome ))

            #******Sort samples from best to worst acourding to fitness evaluation***************
            samples.sort(key=key,reverse=True)

            #********Update probability vectors towards best solutions*********
            for j in range(self.numOfLearningVectors):
                for i in range(self.chromosomeLength):
                    self.probabilityVector[i] = self.probabilityVector[i] * (1-self.learningRate) + samples[j].chromosome[i] * self.learningRate;

        def Run(self, numberOfCycles):
            self.Initialize();
            for i in range(numberOfCycles):
                self.RunCycle();
            return self.probabilityVector;


def RunPBIL(TValue=5,cycles=1500,numberOfSamples=200,learningRate=.005,numOfLearningVectors=2, chromosomeLength=100):
    pbil = PBILAlgorithm(TFitnessFunction(TValue),numberOfSamples=numberOfSamples,learningRate=learningRate,numOfLearningVectors=numOfLearningVectors, chromosomeLength=chromosomeLength)
    p = pbil.Run(cycles)
    print(p)
    plt.plot(p)
    plt.show()



RunPBIL()
