using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class square
{
    public Dictionary<string, float> colonySize = new Dictionary<string, float>();
    public Dictionary<string, int> speciesCount = new Dictionary<string, int>();
    List<string> keys = new List<string>();
   
    public void Add(float size, string key)
    {

        if(keys.BinarySearch(key) < 0)
        {
            keys.Add(key);
            speciesCount.Add(key, 1);
            colonySize.Add(key, size);
        }
        else
        {
            speciesCount[key]++;
            colonySize[key] += size;
        }
    }
    public void Delete(float size, string key)
    {

        if (keys.BinarySearch(key) >= 0)
        {
            if(!IsEmpty(key))
            {
                speciesCount[key]--;
                colonySize[key] -= size;
            }
            else
            {
                keys.Remove(key);
                speciesCount.Remove(key);
                colonySize.Remove(key);
            }

        }
    }
    public bool GetIsOverHalfByNormalize(string target_key)
    {
        if (!colonySize.ContainsKey(target_key))
        {
            return false;
        }
        float sum = 0;
        foreach(string key in keys)
        {
            sum += colonySize[key];
        }
        if (colonySize[target_key] / sum > 0.5f)
        {
            return true;
        }
        return false;
    }
    public float GetNormalizedValue(string target_key)
    {
        if (!colonySize.ContainsKey(target_key))
        {
            return 0;
        }
        float sum = 0;

        foreach (string key in keys)
        {
                sum += colonySize[key];
            
          
        }
        return colonySize[target_key] / sum;
    }
    public int GetCountOfSpecies(string target_key)
    {  if(!speciesCount.ContainsKey(target_key))
        {
            return 0;
        }
        return speciesCount[target_key];
    }
    public bool IsEmpty(string key)
    {
        if (!speciesCount.ContainsKey(key))
        {
            return true;
        }
        return speciesCount[key] <= 0;
    }
}
public class TacticController : MonoBehaviour
{
    // Start is called before the first frame update
    public int size = 10;
    square[,] squareList;
    public List<string> keys;
    Collider squareSpace;
    public GameObject targetObj = null;
    void Start()
    {
        squareList = new square[size, size];
        for (int raw = 0; raw < size; raw++){
            for (int column = 0; column < size; column++)
            {
                squareList[raw, column] = new square();
            }
        }
        keys = new List<string>();
        squareSpace = GetComponent<Collider>();
        gameObject.transform.localScale = new Vector3(size, 1.0f, size);
    }
    void OnTriggerEnter(Collider other)
    {
    
        targetObj = other.gameObject;
    }
    void OnTriggerExit(Collider other)
    {
        Destroy(other.gameObject);
        targetObj = null;
    }
    public void Add(GameObject target)
    {
        
            if (keys.BinarySearch(target.name) < 0)
            {
                keys.Add(target.name);
            }
            target.transform.parent = gameObject.transform;
            Vector2 normTarget = new Vector2(target.transform.localPosition.x, target.transform.localPosition.z);
            int raw = (int)Mathf.Round(normTarget.x * size) + (size / 2);

            int column = (int)Mathf.Round(normTarget.y *size ) + (size / 2);
      
       
       squareList[raw, column].Add(target.GetComponent<Collider>().bounds.size.x * target.GetComponent<Collider>().bounds.size.z, target.name);
        gameObject.transform.DetachChildren();
        target.transform.parent = null;

    }
    public void Delete(GameObject target)
    {
        target.transform.parent = gameObject.transform;
        Vector2 normTarget = new Vector2(target.transform.localPosition.x, target.transform.localPosition.z);

        int raw = (int)Mathf.Round(normTarget.x * size) + (size / 2);
       

        int column = (int)Mathf.Round(normTarget.y * size) + (size / 2);
    

        squareList[raw, column].Delete(target.GetComponent<Collider>().bounds.size.x * target.GetComponent<Collider>().bounds.size.z, target.name);
        if (squareList[raw, column].IsEmpty(target.name))
        {
          
            keys.Remove(target.name);
        }
        gameObject.transform.DetachChildren();
        target.transform.parent = null;
    }

    public int GetDensity(string target)
    {
        int Sum = 0;
        foreach (var column in squareList)
        {
            if(column != null)
            {
                Sum += column.GetCountOfSpecies(target);
            }
         
        }
        return Sum;
    }
    public float GetCover(string target)
    {
        float Sum = 0;
        foreach (var column in squareList)
        {
            if (column != null)
            {
                Sum += column.GetNormalizedValue(target);
            }
        }
        return Sum;

    }

    public int GetFrequency(string target)
    {
        int Sum = 0;
        foreach (var column in squareList)
        {
            if (column != null)
            {
                if (column.GetIsOverHalfByNormalize(target))
                {
                    Sum += 4;
                }
                else
                {
                    Sum += 1;
                }
            }
           
        }
        return Sum;
    }

    public float GetRelativeDensity(string target)
    {
        float Sum = 0;
        foreach (string key in keys)
        {
            
                Sum += GetDensity(key);
        }
        if(Sum == 0)
        {
            return 0;
        }
        return GetDensity(target) / Sum;
    }
    public float GetRelativeCover(string target)
    {
        float Sum = 0;
        foreach (string key in keys)
        {
            Sum += GetCover(key);
        }
        if (Sum == 0)
        {
            return 0;
        }
        return GetCover(target) / Sum;
    }
    public float GetRelativeFrequency(string target)
    {
        float Sum = 0;
        foreach (string key in keys)
        {
            Sum += GetFrequency(key);
        }
        if (Sum == 0)
        {
            return 0;
        }
        return GetFrequency(target) / Sum;
    }

}

