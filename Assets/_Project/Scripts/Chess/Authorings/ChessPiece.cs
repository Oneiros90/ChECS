using Unity.Entities;
using UnityEngine;

namespace CHECS
{
    public class ChessPiece : Authoring
    {
        [SerializeField]
        private PieceType _type;

        [SerializeField]
        private PieceColor _color;

        public override void Bake(Entity entity, IBaker baker)
        {
            baker.AddComponent<PieceTag>(entity);
            baker.AddComponent<EvaluateValidMovesTag>(entity);
            baker.AddBuffer<ValidMoveBufferElement>(entity);

            baker.AddComponent(entity, new PieceColorData(_color));
            switch (_color)
            {
                case PieceColor.White:
                    baker.AddComponent<WhiteTag>(entity);
                    break;
                case PieceColor.Black:
                    baker.AddComponent<BlackTag>(entity);
                    break;
            }

            switch (_type)
            {
                case PieceType.King:
                    baker.AddComponent<KingTag>(entity);
                    break;
                case PieceType.Queen:
                    baker.AddComponent<QueenTag>(entity);
                    break;
                case PieceType.Rook:
                    baker.AddComponent<RookTag>(entity);
                    break;
                case PieceType.Bishop:
                    baker.AddComponent<BishopTag>(entity);
                    break;
                case PieceType.Knight:
                    baker.AddComponent<KnightTag>(entity);
                    baker.AddComponent<CanJumpOtherPiecesTag>(entity);
                    break;
                case PieceType.Pawn:
                    baker.AddComponent<PawnTag>(entity);
                    break;
            }
        }

        public override void Initialize(Entity entity, EntityManager entityManager)
        {
            var localPosition = transform.localPosition;
            entityManager.AddComponentData(entity, new PositionData((sbyte)localPosition.x, (sbyte)localPosition.z));
        }
    }
}