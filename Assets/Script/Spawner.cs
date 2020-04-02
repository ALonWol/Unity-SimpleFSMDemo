using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] posLst;

    public GameObject gruntPrefab;

    int index;

    float INTERVAL => 1f;

    int MAX_COUNT => 4;

    List<GameObject> pool;

    Dictionary<int, GameObject> cache;

    bool startGenerate;

    void Awake() {
        
        pool = new List<GameObject>();
        cache = new Dictionary<int, GameObject>();

        // generate one immediately
        // GenerateGrunt();

        // sequence generate with interval
        TryStartGenerate();
    }

    int count = 0;
    void GenerateGrunt() {
        if (count >= MAX_COUNT) {
            // Debug.Log("count >= MAX:" + count);
            StopGenerate();
            return;
        }

        index = UnityEngine.Random.Range(0, posLst.Length);
        if (cache.ContainsKey(index)) {
            GenerateGrunt();
            return;
        }

        var dir = transform.position - posLst[index].position;
        var angle = 90f - Mathf.Atan2(dir.z, dir.x) * 57.29578f/*PI / 180*/;
        var grunt = pool.Count > 0 ? pool[0] : null;
        if (grunt == null) {
            grunt = GameObject.Instantiate<GameObject>(gruntPrefab, posLst[index].position, Quaternion.Euler(0, angle, 0));
        } else {
            pool.Remove(grunt);
        }

        if (grunt) {
            grunt.transform.SetParent(transform.parent);
            grunt.SetActive(true);
            cache[index] = grunt;
            count++;
            // Debug.Log("[Spawner] - GenerateGruntInternal(): index:" + index + " pos:" + posLst[index].position);
        }
    }

    public void KillGrunt(GameObject grunt) {
        // Debug.Log("KillGrunt:" + grunt);
        if (grunt) {
            grunt.SetActive(false);
            grunt.transform.SetParent(null);

            foreach (var kv in cache) {
                if (kv.Value == grunt) {
                    cache.Remove(kv.Key);
                    break;
                }
            }

            if (!pool.Contains(grunt)) {
                pool.Add(grunt);
            }
        }
    }

    void TryStartGenerate() {
        if (cache.Count < MAX_COUNT && !startGenerate) {
            InvokeRepeating("GenerateGrunt", 1, INTERVAL);
            startGenerate = true;
        }
    }

    void StopGenerate() {
        CancelInvoke("GenerateGrunt");
        startGenerate = false;
    }
}
