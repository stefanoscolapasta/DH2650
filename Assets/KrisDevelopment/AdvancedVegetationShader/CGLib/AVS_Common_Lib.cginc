#ifndef AVS_COMMON_LIB
#define AVS_COMMON_LIB

half4 _AVS_CharacterPosition;


void ApplyCharacterInteraction_half(half3 interactableWorldPos, half originDistance, half interactionAngle, out half3 output)
{
	half _rad = _AVS_CharacterPosition.w;
	half3 _direction = interactableWorldPos - _AVS_CharacterPosition.xyz;
	half3 _flat = half3(_direction.x, 0, _direction.z);
	output = interactableWorldPos + normalize(_flat) * (originDistance * interactionAngle * saturate(_rad - length(_direction)));
}


void VertexMod_half(half4 worldPos, half4 originWp, half speed, half amount, half dst, half zmotion, half zmotionSpeed, half originWeight, half interaction, half time, out half4 output)
{
	half _DistanceFromOrigin = distance(originWp.y, worldPos.y);
	half _zmotionSpd = zmotionSpeed / 10;

	half _noise = sin(time * speed + (worldPos.y + originWp.x + originWp.z) * amount);
	half _noiseZMotion = sin(time * _zmotionSpd + worldPos.y * amount * _zmotionSpd);

	half _anchored = _noise * dst * (_DistanceFromOrigin / 3);
	half _unanchored = _noise * dst;
	half _nxz = zmotion * _noiseZMotion;
	half _lerp = lerp(_unanchored, _anchored, originWeight);

	worldPos.x += _lerp * (1 - _nxz);
	worldPos.z += _lerp * _nxz;

	half3 _pos;
	ApplyCharacterInteraction_half(worldPos.xyz, _DistanceFromOrigin, interaction, _pos);

	output = float4(_pos.x, _pos.y, _pos.z, worldPos.w);
}

void WorldNormalMod_half(half3 worldNormal, half uniformLerp, out half3 output)
{
	output = lerp(worldNormal, half3(0,1,0), uniformLerp);
}
#endif