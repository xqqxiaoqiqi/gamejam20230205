Shader "PCGTerrain/ProceduralTileTerrain"
{
    Properties
        {
            _MainTex ("Texture", 2D) = "white" {}
            _NoiseOffset("_NoiseOffset",vector) = (0,0,0,0)
            _NoiseFreq("frequncy",Range(0,50))=1
            
            _NoiseStep("noiseStep",Range(0,10)) = 2
            _NoisePersistence("noisePersist",Range(0,5))=1
            _TerrainTiles_x("TerrainTilesx（百）",Range(0,10)) = 1
            _TerrainTiles_y("TerrainTilesy（百）",Range(0,10)) = 1
            _LargeScale("_LargeScale",Range(0,12)) = 0.1
            _LargeBlur("_LargeBlur",Range(0,1)) = 1
            _LargeFac("_LargeFac(Pow)",Range(0,10)) = 0.1
            _DetailFac("detailfac",Range(0,0.5)) = 0.1
            _Color_Mount("Color_Mount",Color) = (1,1,1,1)
            _Mount("mount",Range(0.8,1)) = 0.9
            _Color_Hill("Color_Hill",Color) = (1,1,1,1)
            _Hill("mount",Range(0,1)) = 0.7
            _Color_NormalHeight("COlor_NormalHeight",Color) = (1,1,1,1)
            _NormalHeight("mount",Range(0,1)) = 0.4
            _Color_Water("Color_Water",Color) = (1,1,1,1)
            //
            _Color_Sand("Color_Sand",Color) = (1,1,1,1)
            _Sand("sand",Range(0.6,1)) = 0.1
            _Color_Ice("Color_Ice",Color) = (1,1,1,1)
            _Ice("ice",Range(0,0.4)) = 0.1
            
            _Color_Marsh("Color_Marsh",Color) = (1,1,1,1)
            _Color_Forest("Color_Forest",COlor)=(0,1,0,1)
            _MarshForestRange("MarshForestRange",vector) = (0,0.4,0.6,0.7)
            _SpecialNoiseFac("_SpecialNoiseFac",Range(0,0.5)) = 0.1
            _SpecialNoiseScale("_SpecialNoisescale",Range(0,9))=1
            
        }
        SubShader
        {
            Tags { "RenderType"="Opaque" }
    
            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
    
                #include "UnityCG.cginc"
    
                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };
    
                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };
    
                sampler2D _MainTex;
                float4 _MainTex_ST;
                float4 _NoiseOffset;
                float _NoiseFreq;
                int _NoiseStep;
                float _NoisePersistence;
                float _TerrainTiles_x;
                float _TerrainTiles_y;
                float _Mount;
                float _Hill;
                float _NormalHeight;
                float _LargeScale;
                float _LargeFac;
                float _LargeBlur;
                float _DetailFac;
                //
                float _Sand;
                float _Ice;
                float _SpecialNoiseFac;
                float _SpecialNoiseScale;
                float4 _MarshForestRange;
    
                float4 _Color_Mount,_Color_Hill,_Color_NormalHeight,_Color_Water,_Color_Sand,_Color_Ice,_Color_Marsh,_Color_Forest;
    
                float hash1( float n ) { return frac(sin(n)*43758.5453); }
                float2  hash2( float2  p ) { p = float2( dot(p,float2(127.1,311.7)), dot(p,float2(269.5,183.3)) ); return frac(sin(p)*43758.5453); }
    
                //Voronoi
                float3 hash3(float2 p )
                {
                    float3 q = float3( dot(p,float2(127.1,311.7)), 
                				   dot(p,float2(269.5,183.3)), 
                				   dot(p,float2(419.2,371.9)) );
                	return frac(sin(q)*43758.5453);
                }
                
                float4 voronoi(float2 x, float w )
                {
                    float2 n = floor( x );
                    float2 f = frac( x );
                
                	float4 m = float4( 8.0, 0.0, 0.0, 0.0 );
                    for( int j=-2; j<=2; j++ )
                    for( int i=-2; i<=2; i++ )
                    {
                        float2 g = float2( float(i),float(j) );
                        float2 o = hash2( n + g );
                		
                		// animate
                        o = 0.5 + 0.5*sin(6.2831*o );
                
                        // distance to cell		
                		float d = length(g - f + o);
                		
                        // cell color
                		float3 col = 0.5 + 0.5*sin( hash1(dot(n+g,float2(7.0,113.0)))*2.5 + 3.5 + float3(3.0,3.0,3.0));
                        // in linear space
                        col = col*col;
                        
                        // do the smooth min for colors and distances		
                		float h = smoothstep( -1.0, 1.0, (m.x-d)/w );
                	    m.x   = lerp( m.x,     d, h ) - h*(1.0-h)*w/(1.0+3.0*w); // distance
                		m.yzw = lerp( m.yzw, col, h ) - h*(1.0-h)*w/(1.0+3.0*w); // color
                    }
                	
                	return m;
                }
                
                //perlin
                
                float rand(float2 co)
                {
                    return frac(sin(dot(co.xy ,float2(12.9898,78.233))) * 43758.5453);
                }
    
                float hermite(float t)
                {
                  return t * t * (3.0 - 2.0 * t);
                }
    
                float noise(float2 co, float frequency)
                {
                  float2 v = float2(co.x * frequency, co.y * frequency);
                
                  float ix1 = floor(v.x);
                  float iy1 = floor(v.y);
                  float ix2 = floor(v.x + 1.0);
                  float iy2 = floor(v.y + 1.0);
                
                  float fx = hermite(frac(v.x));
                  float fy = hermite(frac(v.y));
                
                  float fade1 = lerp(rand(float2(ix1, iy1)), rand(float2(ix2, iy1)), fx);
                  float fade2 = lerp(rand(float2(ix1, iy2)), rand(float2(ix2, iy2)), fx);
                
                  return lerp(fade1, fade2, fy);
                }
                
                float pnoise(float2 co, float freq, int steps, float persistence)
                {
                  float value = 0.0;
                  float ampl = 1.0;
                  float sum = 0.0;
                  for(int i=0 ; i<steps ; i++)
                  {
                    sum += ampl;
                    value += noise(co, freq) * ampl;
                    freq *= 2.0;
                    ampl *= persistence;
                  }
                  return value / sum;
                }
    
                v2f vert (appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    return o;
                }
    
                half4 frag (v2f i) : SV_Target
                {
                    // sample the texture
                    half4 col = 1;
                    float2 _TerrainTiles = float2(_TerrainTiles_x,_TerrainTiles_y);
                    float2 uv = floor((i.uv+_NoiseOffset.xy)*_TerrainTiles.xy*100)/(_TerrainTiles.xy*100);
                    float detailNoise = pnoise((uv),_NoiseFreq,_NoiseStep,_NoisePersistence).rrr;
                    float largeNoise = 1 - voronoi((uv+_NoiseOffset.xy)*_LargeScale,0.3);
                    largeNoise = largeNoise-0.5f;
                    largeNoise = 0.5+sign(largeNoise)*pow(abs(largeNoise),_LargeFac);
                    //return half4(saturate(largeNoise.rrr),1);
                    col.rgb  = saturate((detailNoise*2-1)*_DetailFac + largeNoise);
                    float height = col.r;
                    float mheight = height + (pnoise((uv.yx+_MarshForestRange.x),_NoiseFreq,_NoiseStep,_NoisePersistence).r*2-1)*0.1f;
                    float fheight = (pnoise((uv.yx+_MarshForestRange.z),_NoiseFreq,_NoiseStep,_NoisePersistence).r);
                    if (height> _Mount)
                    {
                        col.rgb = _Color_Mount;
                    }
                    else if (height >_Hill)
                    {
                        col.rgb = _Color_Hill;
                    }
                    else if (height > _NormalHeight)
                    {
                        col.rgb = _Color_NormalHeight;
                    }
                    else
                    {
                        col.rgb = _Color_Water;
                    }
                    float2 absoluteUV = floor((i.uv)*_TerrainTiles.xy*100)/(_TerrainTiles.xy*100);
                    float specialNoise = pnoise((absoluteUV),_SpecialNoiseScale,_NoiseStep,1).rrr;
                    if (mheight<_MarshForestRange.y && length(col.rgb - _Color_NormalHeight)<0.01f)
                    {
                        col.rgb = _Color_Marsh;
                    }
                    if (fheight<_MarshForestRange.w && length(col.rgb-_Color_NormalHeight)<0.01f)
                    {
                        col.rgb = _Color_Forest;
                    }
                    if (absoluteUV.y + (specialNoise*2-1)*_SpecialNoiseFac>_Sand)
                    {
                        col.rgb = _Color_Sand;
                    }
                    if (absoluteUV.y+(specialNoise*2-1)*_SpecialNoiseFac<_Ice && height>_NormalHeight)
                    {
                        col.rgb = _Color_Ice;
                    }
                    return col;
                }
                ENDCG
            }
        }
}
