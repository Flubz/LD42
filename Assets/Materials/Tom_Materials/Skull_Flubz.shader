// Upgrade NOTE: upgraded instancing buffer 'Skull' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Skull"
{
	Properties
	{
		_Albedo("Albedo", Color) = (1,1,1,0)
		_AOIntensity("AO Intensity", Range( 0 , 2)) = 1.31
		_FreScale("FreScale", Float) = 1
		_FreBias("FreBias", Range( 0 , 2)) = 1
		_FrePower("FrePower", Float) = 1
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_EmissionColor("EmissionColor", Color) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma multi_compile_instancing
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
			float2 uv_texcoord;
		};

		uniform float _FreScale;
		uniform float _FrePower;
		uniform float4 _EmissionColor;
		uniform sampler2D _TextureSample1;
		uniform float4 _TextureSample1_ST;

		UNITY_INSTANCING_BUFFER_START(Skull)
			UNITY_DEFINE_INSTANCED_PROP(float4, _Albedo)
#define _Albedo_arr Skull
			UNITY_DEFINE_INSTANCED_PROP(float, _AOIntensity)
#define _AOIntensity_arr Skull
			UNITY_DEFINE_INSTANCED_PROP(float, _FreBias)
#define _FreBias_arr Skull
		UNITY_INSTANCING_BUFFER_END(Skull)

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 _Albedo_Instance = UNITY_ACCESS_INSTANCED_PROP(_Albedo_arr, _Albedo);
			o.Albedo = _Albedo_Instance.rgb;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float _FreBias_Instance = UNITY_ACCESS_INSTANCED_PROP(_FreBias_arr, _FreBias);
			float fresnelNdotV20 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode20 = ( _FreBias_Instance + _FreScale * pow( 1.0 - fresnelNdotV20, _FrePower ) );
			o.Emission = ( fresnelNode20 * _EmissionColor ).rgb;
			float2 uv_TextureSample1 = i.uv_texcoord * _TextureSample1_ST.xy + _TextureSample1_ST.zw;
			float _AOIntensity_Instance = UNITY_ACCESS_INSTANCED_PROP(_AOIntensity_arr, _AOIntensity);
			o.Occlusion = ( tex2D( _TextureSample1, uv_TextureSample1 ) * _AOIntensity_Instance ).r;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float3 worldNormal : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.worldNormal = worldNormal;
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = IN.worldNormal;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=15401
1932;60;1906;1044;789.2356;533.8956;1.221937;True;True
Node;AmplifyShaderEditor.RangedFloatNode;22;-895,-511;Float;False;InstancedProperty;_FreBias;FreBias;5;0;Create;True;0;0;False;0;1;1;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;26;-893,-423;Float;False;Property;_FreScale;FreScale;4;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;27;-896,-337;Float;False;Property;_FrePower;FrePower;6;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-787.2399,255.5673;Float;True;Property;_TextureSample1;Texture Sample 1;9;0;Create;True;0;0;False;0;1b1aad2617dc42f478465e4bf323a0f5;1b1aad2617dc42f478465e4bf323a0f5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;4;-656.6393,459.5509;Float;False;InstancedProperty;_AOIntensity;AO Intensity;3;0;Create;True;0;0;False;0;1.31;2;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;20;-531.1858,-381.939;Float;True;Standard;WorldNormal;ViewDir;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;31;139.3829,331.9894;Float;False;Property;_EmissionColor;EmissionColor;10;0;Create;True;0;0;False;0;0,0,0,0;0.4632353,0.4257677,0.4257677,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleContrastOpNode;24;210.4315,13.03341;Float;False;2;1;COLOR;0,0,0,0;False;0;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ScreenDepthNode;28;206.7056,143.6859;Float;False;0;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;510.9052,297.0213;Float;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;23;-56.49992,-253.9289;Float;False;InstancedProperty;_Albedo;Albedo;2;0;Create;True;0;0;False;0;1,1,1,0;1,0.8602941,0.8602941,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-256.5868,257.7698;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;416,17.22194;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;1;-912.7721,-4.214674;Float;True;Property;_TextureSample0;Texture Sample 0;8;0;Create;True;0;0;False;0;b2235d9a89678204b9cfa05d87819240;b2235d9a89678204b9cfa05d87819240;True;0;False;gray;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;13;-845.6379,-246.8441;Float;False;InstancedProperty;_Emissive;Emissive;1;0;Create;True;0;0;False;0;1,0.3823529,0.3823529,0;1,0.2279412,0.2279412,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;18;-1494.079,-0.4215574;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;5,5;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;19;-1182.781,-2.716068;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,-0.5;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-463.7408,11.88278;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;25;-122.7875,133.0132;Float;False;InstancedProperty;_ABBERATION;ABBERATION;7;0;Create;True;0;0;False;0;1;1;-5;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;2.089118,4.785349;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-241.6105,11.88275;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-475.889,122.0137;Float;False;Constant;_Float1;Float 1;3;0;Create;True;0;0;False;0;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;768,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Skull;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Masked;0.5;True;True;0;False;TransparentCutout;;AlphaTest;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;20;1;22;0
WireConnection;20;2;26;0
WireConnection;20;3;27;0
WireConnection;24;1;21;0
WireConnection;24;0;25;0
WireConnection;28;0;21;0
WireConnection;32;0;20;0
WireConnection;32;1;31;0
WireConnection;3;0;2;0
WireConnection;3;1;4;0
WireConnection;29;0;24;0
WireConnection;29;1;28;0
WireConnection;1;1;19;0
WireConnection;19;0;18;0
WireConnection;7;0;13;0
WireConnection;7;1;1;0
WireConnection;21;1;8;0
WireConnection;8;0;7;0
WireConnection;8;1;9;0
WireConnection;0;0;23;0
WireConnection;0;2;32;0
WireConnection;0;5;3;0
ASEEND*/
//CHKSM=2ED5C9A084D1C6C17B8218292472D315AB2B0F7D