using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Controller.General;
using _Scripts.Models.Ground;
using _Scripts.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Scripts.Controller.Ground
{
    public class GroundGenerator
    {
        private const float ChunkSizeZ = 2f;
        
        public GameObject GenerateGround(int length = 15, GroundDifficulty difficulty = GroundDifficulty.Easy, int level = 1)
        {
            var chunksData = GameHub.Instance.ChunksData;
            
            // create empty game object that will hold all chunks
            var ground = new GameObject("Ground");
            ground.transform.position = Vector3.zero;
            
            if (length < 10) length = 10;
            if (length > 100) length = 100;
            
            // create three empty chunks
            var counter = 1;
            var currentPos = Vector3.zero;
            var emptyChunk = chunksData.chunks[0].chunk;
            while (counter <= 3)
            {
                var chunk1 = Object.Instantiate(emptyChunk, currentPos, Quaternion.identity);
                chunk1.name = $"Chunk {counter}";
                chunk1.transform.parent = ground.transform;
                counter++;
                currentPos.z += ChunkSizeZ;
            }

            var lastObstaclePos = 0;
            var lastResourcePos = 0;
            var lastChoicePos = 0;

            var difficultyData = difficulty switch
            {
                GroundDifficulty.Easy => chunksData.easyDifficulty,
                GroundDifficulty.Medium => chunksData.mediumDifficulty,
                GroundDifficulty.Hard => chunksData.hardDifficulty,
                _ => chunksData.easyDifficulty
            };

            var choicesRemaining = Mathf.Floor(level / 2f) + 1;
            var minDistBtwnChoice = Mathf.Floor((length-2) / choicesRemaining) + 1;
            

            while (counter < length)
            {
                // get chunk type based on difficulty
                var validChunkTypes = new List<ChunkType>()
                {
                    ChunkType.Empty,
                    ChunkType.Obstacle,
                    ChunkType.Resource,
                    ChunkType.Choice
                };
                
                if (counter - lastObstaclePos < difficultyData.minObstacleDistance)
                {
                    validChunkTypes.Remove(ChunkType.Obstacle);
                }
                
                if (counter - lastResourcePos < difficultyData.minResourceDistance)
                {
                    validChunkTypes.Remove(ChunkType.Resource);
                }
                
                if (choicesRemaining <= 0 || counter - lastChoicePos < minDistBtwnChoice)
                {
                    validChunkTypes.Remove(ChunkType.Choice);
                }
                
                var chunkType = validChunkTypes[UnityEngine.Random.Range(0, validChunkTypes.Count)];
                
                // create chunk
                var chunk = chunksData.chunks.FindAll(x=>x.type == chunkType).Random();
                var chunkObj = Object.Instantiate(chunk.chunk, currentPos, Quaternion.identity);
                chunkObj.name = $"Chunk {counter}";
                chunkObj.transform.parent = ground.transform;
                
                // update last chunk positions
                switch (chunkType)
                {
                    case ChunkType.Obstacle:
                        lastObstaclePos = counter;
                        break;
                    case ChunkType.Resource:
                        lastResourcePos = counter;
                        break;
                    case ChunkType.Choice:
                        lastChoicePos = counter;
                        choicesRemaining -= 1;
                        break;
                }
                
                counter++;
                currentPos.z += ChunkSizeZ;
            }
            
            // create finish chunk
            var finishChunk = chunksData.chunks.Find(x=>x.type == ChunkType.Finish).chunk;
            var finishChunkObj = Object.Instantiate(finishChunk, currentPos, Quaternion.identity);
            finishChunkObj.name = $"Chunk {counter}";
            finishChunkObj.transform.parent = ground.transform;
            
            return ground;
        }
    }
}