using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilityStatic
{
    /// <summary>
    /// <para>This method will return an outcome if invoked along with an array of probability values.</para>
    /// <para>E.g. If we have an array of animations and we want to select one among them according to a probability destribution, we would do the following:</para>
    /// <para>
    ///     AnimationClip[] clips = {clip1, clip2, clip3};<para/>
    ///     float[] probability = {20, 70, 10}; //Probability distribution of 20%, 70% and 10%<para/>
    ///     AnimationClip clip = UtilityStatic.getOutCome(clips, probability);
    /// </para>
    /// <para>
    /// Upon hundred invokations, this would result in clip1 being selected 20 times, clip2 70 and clip3 10 times.
    /// </para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <param name="probArray"></param>
    /// <returns></returns>
    public static T getOutCome<T>(T[] t, float[] probArray)
    {
        int retValue = 0;
        float total = 0;
        foreach (int element in probArray)
        {
            total += element;
        }

        float randomPoint = Random.value * total;
        for (int i = 0; i < probArray.Length; i++)
        {
            if (randomPoint < probArray[i])
            {
                retValue = i;
                break;
            }
            else
            {
                randomPoint -= probArray[i];
            }
        }
        return t[retValue];
    }

}
