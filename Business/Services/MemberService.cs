using Database.Repos;
using Domain.Dtos;
using Domain.Extensions;

namespace Business.Services;

public class MemberService
{
    private readonly MemberRepository _memberRepository;

    public MemberService(MemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<IEnumerable<MemberDto>> GetAllMembersAsync()
    {
        var members = await _memberRepository.GetAllAsync();

        // Om MapTo() fungerar korrekt och mappar från ClientEntity till ClientDto:
        return members.Result.Select(member => member.MapTo<MemberDto>()).ToList();
    }
}
