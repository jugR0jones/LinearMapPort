using System.Diagnostics;

namespace LinearMapPort;

public class LinearMap
{
    #region Data Structures

    private struct LinearMapStruct
    {
        public float inputValue;
        public float outputValue;
    }

    #endregion

    #region Private Fields

    private List<LinearMapStruct> m_linearMapArray;

    #endregion

    #region Constructors

    public LinearMap()
    {
        m_linearMapArray = new List<LinearMapStruct>();
    }

    #endregion

    #region Public Methods

    // inputValue must be added in ascending order
    public void Add(in float inputValue, in float outputValue)
    {
        LinearMapStruct map = new LinearMapStruct()
        {
            inputValue =  inputValue,
            outputValue = outputValue
        };

        
        if (GetCount() > 0)
        {
            Debug.Assert(m_linearMapArray[m_linearMapArray.Count - 1].inputValue < inputValue);
        }

        m_linearMapArray.Add(map);
    }

    public float Get(in float input)
    {
        Debug.Assert(m_linearMapArray.Count > 0);
        
        // check lower bound
        LinearMapStruct minimum = m_linearMapArray[0];
        if (input < minimum.inputValue)
        {
            return minimum.outputValue;
        }

        // check high bound
        LinearMapStruct maximum = m_linearMapArray[m_linearMapArray.Count - 1];
        if (input > maximum.inputValue)
        {
            return maximum.outputValue;
        }
        // iterate through list

        //leastMax equals the largest positive difference between input and the largest LinearMap::value
        int leastMaxIndex = 0;
        int leastMinIndex = 0;
        float leastMax = maximum.inputValue - input + 1;
        float leastMin = minimum.inputValue - input - 1;

        for(int i=0; i < m_linearMapArray.Count; i++)
        {
            LinearMapStruct linearMap = m_linearMapArray[i];
            // if we have an exact match
            if (Math.Abs(linearMap.inputValue - input) < float.Epsilon)
            {
                return linearMap.outputValue;
            }

            // if no match search for the closest boundary points
            float inputValueDelta = linearMap.inputValue - input;
            if (inputValueDelta > 0.0f &&
                inputValueDelta < leastMax)
            {
                leastMaxIndex = i;
                leastMax = inputValueDelta;
            }

            if (inputValueDelta < 0.0f &&
                input - linearMap.inputValue > leastMin)
            {
                leastMinIndex = i;
                leastMin = inputValueDelta;
            }
        }

        // calculate the value using the leastMinIndex and leastMaxIndex
        LinearMapStruct leastMinimum = m_linearMapArray[leastMinIndex];
        LinearMapStruct leastMaximum = m_linearMapArray[leastMaxIndex];
        
        return CalculateOutputValue(in leastMinimum.outputValue,
            in leastMaximum.outputValue,
            in leastMinimum.inputValue,
            in leastMaximum.inputValue,
            in input);
    }

    public float GetInputFromOutput(in float outputValue)
    {
        Debug.Assert(m_linearMapArray.Count > 0);
        
        // check lower bound
        if (outputValue < m_linearMapArray[0].outputValue)
        {
            //    KLOG( Warning, KString() << "The value: " << outputValue << "is lower than the range in KLinearMap" );
            return m_linearMapArray[0].inputValue;
        }

        // check high bound
        if (outputValue > m_linearMapArray[m_linearMapArray.Count - 1].outputValue)
        {
            //    KLOG( Warning, KString() << "The value: " << outputValue << "is greater than the range in KLinearMap" );
            return m_linearMapArray[m_linearMapArray.Count - 1].inputValue;
        }
        // iterate through list

        //leastMax equals the largest positive difference between outputValue and the largest LinearMap::value
        int leastMaxIndex = 0;
        int leastMinIndex = 0;
        
        float leastMax = m_linearMapArray[m_linearMapArray.Count - 1].outputValue - outputValue + 1;
        float leastMin = m_linearMapArray[0].outputValue - outputValue - 1;
        
        for(int i=0; i < m_linearMapArray.Count; i++)
        {
            LinearMapStruct linearMap = m_linearMapArray[i];
            
            // if we have an exact match
            if (Math.Abs(linearMap.outputValue - outputValue) < float.Epsilon)
            {
                return linearMap.inputValue;
            }

            // if no match search for the closest boundary points
            float linearMapOutputValueDelta = linearMap.outputValue - outputValue;
            if (linearMapOutputValueDelta > 0.0f &&
                linearMapOutputValueDelta < leastMax)
            {
                leastMaxIndex = i;
                leastMax = linearMapOutputValueDelta;
            }

            if (linearMapOutputValueDelta < 0.0 &&
                outputValue - linearMap.outputValue > leastMin)
            {
                leastMinIndex = i;
                leastMin = linearMapOutputValueDelta;
            }
        }

        // calculate the value using the leastMinIndex and leastMaxIndex
        LinearMapStruct leastMinimum = m_linearMapArray[leastMinIndex];
        LinearMapStruct leastMaximum = m_linearMapArray[leastMaxIndex]; 
        
        return CalculateInputValue(in leastMinimum.outputValue, 
            in leastMaximum.outputValue,
            in leastMinimum.inputValue,
            in leastMaximum.inputValue,
            in outputValue);
    }

    public int GetCount()
    {
        return m_linearMapArray.Count;
    }

    //return -1 if does not find input value in map and index value if input value exists
    public int InputValueExistsInMap(in float inputValue)
    {
        for (int i = 0; i < m_linearMapArray.Count; i++)
        {
            if (Math.Abs(m_linearMapArray[i].inputValue - inputValue) < float.Epsilon)
            {
                return i;
            }
        }

        return -1;
    }

    //replaces raw value with associated Cal value if Cal value exists in linear map
    public void ReplaceInputValueMap(in float inputValue, in float outputValue)
    {
        int index = InputValueExistsInMap(inputValue);
        if (index == -1)
        {
            return;
        }
        
        LinearMapStruct linearMap = m_linearMapArray[index];
        linearMap.outputValue = outputValue;
        m_linearMapArray[index] = linearMap;
    }

    /// <summary>
    /// Returns Calibrated Value at specified index
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public float GetInputValueAtIndex(in int index)
    {
#if DEBUG
        
        Debug.Assert(index >= 0 && index < m_linearMapArray.Count);
#endif
        
        return m_linearMapArray[index].inputValue;
    }

    /// <summary>
    /// Returns Raw Value at specified index
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public float GetOutputValueAtIndex(in int index)
    {
#if DEBUG
        
        Debug.Assert(index >= 0 && index < m_linearMapArray.Count);
        
#endif

        return m_linearMapArray[index].outputValue;
    }

    public void SetOutputValueAtIndex(in int index, in float outputValue)
    {
#if DEBUG
        
        Debug.Assert(index >= 0 && index < m_linearMapArray.Count);
        
#endif
        
        LinearMapStruct tmpLinearMap = m_linearMapArray[index];
        tmpLinearMap.outputValue = outputValue;
        m_linearMapArray[index] = tmpLinearMap;
    }

    public float GetMinOutput()
    {
        return m_linearMapArray[0].outputValue;
    }

    public float GetMaxOutput()
    {
        return m_linearMapArray[m_linearMapArray.Count - 1].outputValue;
    }

    #endregion

    #region Private Methods

    private static float CalculateOutputValue(in float outputMin, in float outputMax, in float inputMin, in float inputMax, in float inputVal)
    {
        float range = (outputMax - outputMin) / (inputMax - inputMin);
        return outputMin + (inputVal - inputMin) * range;
    }

    private static float CalculateInputValue(in float outputMin, in float outputMax, in float inputMin, in float inputMax, in float outputValue)
    {
        float range = (inputMax - inputMin) / (outputMax - outputMin);
        return inputMin + (outputValue - outputMin) * range;
    }

    #endregion
}