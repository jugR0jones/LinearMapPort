using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace LinearMapPort;

public class FastLinearMapWithMultiplePoints
{
    #region Private Fields

    private float[] inputArray;
    private float[] outputArray;

    private int capacity;
    private int numberOfItems;

    #endregion

    #region Constructors

    public FastLinearMapWithMultiplePoints()
    {
        capacity = 4;
        numberOfItems = 0;
        
        inputArray = new float[capacity];
        outputArray = new float[capacity];
    }

    #endregion

    #region Public Methods

    // inputValue must be added in ascending order
    public void Add(in float inputValue, in float outputValue)
    {
        if (numberOfItems + 1 > capacity)
        {
            capacity *= 2;
            float[] tmpInputArray = new float[capacity];
            Array.Copy(inputArray, 0, tmpInputArray, 0, numberOfItems);
            inputArray = tmpInputArray;
            
            float[] tmpOutputArray = new float[capacity];
            Array.Copy(outputArray, 0, tmpOutputArray, 0, numberOfItems);
            outputArray = tmpOutputArray;
        }

#if DEBUG
        
        if (numberOfItems > 0)
        {
            // Make sure the last item in the array is less than the new item
            Debug.Assert(inputArray[numberOfItems] < inputValue);
        }
        
#endif
        
        inputArray[numberOfItems] = inputValue;
        outputArray[numberOfItems] = outputValue;
        numberOfItems++;
    }

    public float Get(in float input)
    {
#if DEBUG
        
        Debug.Assert(numberOfItems > 0);
        
#endif
        
        // Check lower bound
        float lowerInputBound = inputArray[0];
        if (input < lowerInputBound)
        {
            return outputArray[0];
        }

        // check high bound
        int indexOfLastItem = numberOfItems - 1;
        float upperInputBound = inputArray[indexOfLastItem];
        if (input > upperInputBound)
        {
            return outputArray[indexOfLastItem];
        }
        
        // iterate through list

        //leastMax equals the largest positive difference between input and the largest LinearMap::value
        int leastMaxIndex = 0;
        int leastMinIndex = 0;
        float leastMax = upperInputBound - input + 1;
        float leastMin = lowerInputBound - input - 1;

        // Order matters. We have to start with 0 and work our way up otherwise end up with a slightly different
        //  value for the output.
        for(int i=0; i < numberOfItems; i++)
        {
            // if we have an exact match
            if (Math.Abs(inputArray[i] - input) < float.Epsilon)
            {
                return outputArray[i];
            }

            // if no match search for the closest boundary points
            float inputValueDelta = inputArray[i] - input;
            if (inputValueDelta > 0.0f &&
                inputValueDelta < leastMax)
            {
                leastMaxIndex = i;
                leastMax = inputValueDelta;

                continue;
            }

            if (inputValueDelta < 0.0f &&
                input - inputArray[i] > leastMin)
            {
                leastMinIndex = i;
                leastMin = inputValueDelta;
            }
        }

        // calculate the value using the leastMinIndex and leastMaxIndex
        return CalculateOutputValue(in outputArray[leastMinIndex],
            in outputArray[leastMaxIndex],
            in inputArray[leastMinIndex],
            in inputArray[leastMaxIndex],
            in input);
    }

    public float GetInputFromOutput(in float outputValue)
    {
        Debug.Assert(numberOfItems > 0);
        
        // check lower bound
        if (outputValue < outputArray[0])
        {
            return inputArray[0];
        }

        // check high bound
        int indexOfLastItem = numberOfItems - 1;
        if (outputValue > outputArray[indexOfLastItem])
        {
            return inputArray[indexOfLastItem];
        }
        // iterate through list

        //leastMax equals the largest positive difference between outputValue and the largest LinearMap::value
        int leastMaxIndex = 0;
        int leastMinIndex = 0;
        
        float leastMax = outputArray[indexOfLastItem] - outputValue + 1;
        float leastMin = outputArray[0] - outputValue - 1;
        
        for(int i=0; i < outputArray.Length; i++)
        {
            // if we have an exact match
            float outputValueFromOutputArray = outputArray[i];
            if (Math.Abs(outputValueFromOutputArray - outputValue) < float.Epsilon)
            {
                return inputArray[i];
            }

            // if no match search for the closest boundary points
            float linearMapOutputValueDelta = outputValueFromOutputArray - outputValue;
            if (linearMapOutputValueDelta > 0.0f &&
                linearMapOutputValueDelta < leastMax)
            {
                leastMaxIndex = i;
                leastMax = linearMapOutputValueDelta;
            }

            if (linearMapOutputValueDelta < 0.0 &&
                outputValue - outputValueFromOutputArray > leastMin)
            {
                leastMinIndex = i;
                leastMin = linearMapOutputValueDelta;
            }
        }

        // Calculate the value using the leastMinIndex and leastMaxIndex
        return CalculateInputValue(in outputArray[leastMinIndex], 
            in outputArray[leastMaxIndex],
            in inputArray[leastMinIndex],
            in inputArray[leastMaxIndex],
            in outputValue);
    }

    public int GetCount()
    {
        return numberOfItems;
    }

    /// <summary>
    /// Return -1 if does not find input value in map and index value if input value exists
    /// </summary>
    /// <param name="inputValue"></param>
    /// <returns></returns>
    public int InputValueExistsInMap(in float inputValue)
    {
        for (int i = numberOfItems - 1; i >= 0; i++)
        {
            if (Math.Abs(inputArray[i] - inputValue) < float.Epsilon)
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// Replaces raw value with associated Cal value if Cal value exists in linear map
    /// </summary>
    /// <param name="inputValue"></param>
    /// <param name="outputValue"></param>
    public void ReplaceInputValueMap(in float inputValue, in float outputValue)
    {
        int index = InputValueExistsInMap(inputValue);
        if (index == -1)
        {
            return;
        }

        inputArray[index] = outputValue;
    }

    /// <summary>
    /// Returns Calibrated Value at specified index
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public float GetInputValueAtIndex(in int index)
    {
#if DEBUG
        
        Debug.Assert(index >= 0 && index < numberOfItems);
        
#endif
        
        return inputArray[index];
    }

    /// <summary>
    /// Returns Raw Value at specified index
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public float GetOutputValueAtIndex(in int index)
    {
#if DEBUG
        
        Debug.Assert(index >= 0 && index < numberOfItems);
        
#endif

        return outputArray[index];
    }

    public void SetOutputValueAtIndex(in int index, in float outputValue)
    {
#if DEBUG
        
        Debug.Assert(index >= 0 && index < numberOfItems);
        
#endif

        outputArray[index] = outputValue;
    }

    public float GetMinOutput()
    {
        return outputArray[0];
    }

    public float GetMaxOutput()
    {
        return outputArray[numberOfItems-1];
    }

    #endregion

    #region Private Methods

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float CalculateOutputValue(in float outputMin, in float outputMax, in float inputMin, in float inputMax, in float inputVal)
    {
        float range = (outputMax - outputMin) / (inputMax - inputMin);
        return outputMin + (inputVal - inputMin) * range;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float CalculateInputValue(in float outputMin, in float outputMax, in float inputMin, in float inputMax, in float outputValue)
    {
        float range = (inputMax - inputMin) / (outputMax - outputMin);
        return inputMin + (outputValue - outputMin) * range;
    }

    #endregion
}