using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuStore.UnitTests.Helpers
{
    internal static class JsonResponseContent
    {
        public static string GoogleMapsApiCity1 => "{\n   \"predictions\" : [\n      {\n         \"description\" : \"Warszawa, Polska\",\n         \"id\" : \"6b893c3d05bf5fb2caa4b033c9aa27a41fab6fc3\",\n         \"matched_substrings\" : [\n            {\n               \"length\" : 8,\n               \"offset\" : 0\n            }\n         ],\n         \"place_id\" : \"ChIJAZ-GmmbMHkcR_NPqiCq-8HI\",\n         \"reference\" : \"ChIJAZ-GmmbMHkcR_NPqiCq-8HI\",\n         \"structured_formatting\" : {\n            \"main_text\" : \"Warszawa\",\n            \"main_text_matched_substrings\" : [\n               {\n                  \"length\" : 8,\n                  \"offset\" : 0\n               }\n            ],\n            \"secondary_text\" : \"Polska\"\n         },\n         \"terms\" : [\n            {\n               \"offset\" : 0,\n               \"value\" : \"Warszawa\"\n            },\n            {\n               \"offset\" : 10,\n               \"value\" : \"Polska\"\n            }\n         ],\n         \"types\" : [ \"locality\", \"political\", \"geocode\" ]\n      },\n      {\n         \"description\" : \"Warsaw, Indiana, Stany Zjednoczone\",\n         \"id\" : \"a0c78f0653f9f965e49e25f3d2ab8514c0e6a72f\",\n         \"matched_substrings\" : [\n            {\n               \"length\" : 6,\n               \"offset\" : 0\n            }\n         ],\n         \"place_id\" : \"ChIJZ7-cQiycFogRAMhnvFnmkbU\",\n         \"reference\" : \"ChIJZ7-cQiycFogRAMhnvFnmkbU\",\n         \"structured_formatting\" : {\n            \"main_text\" : \"Warsaw\",\n            \"main_text_matched_substrings\" : [\n               {\n                  \"length\" : 6,\n                  \"offset\" : 0\n               }\n            ],\n            \"secondary_text\" : \"Indiana, Stany Zjednoczone\"\n         },\n         \"terms\" : [\n            {\n               \"offset\" : 0,\n               \"value\" : \"Warsaw\"\n            },\n            {\n               \"offset\" : 8,\n               \"value\" : \"Indiana\"\n            },\n            {\n               \"offset\" : 17,\n               \"value\" : \"Stany Zjednoczone\"\n            }\n         ],\n         \"types\" : [ \"locality\", \"political\", \"geocode\" ]\n      }\n   ],\n   \"status\" : \"OK\"\n}\n";
        public static string InvalidRequest => "{\n   \"predictions\" : [],\n   \"status\" : \"INVALID_REQUEST\"\n}\n";
    }
}
