namespace Navira.Shop.Application.Media
{
    public sealed class PreparedImage : IAsyncDisposable
    {
        // Streamها باید مستقل، قابل خواندن و از ابتدای محتوا باشند.
        // مالکیت آن‌ها با همین کلاس است.
        public required Stream Original { get; init; }

        public required string OriginalExtension { get; init; }

        public required PreparedImageMetadata Metadata { get; init; }

        public required IReadOnlyList<PreparedImageVariant> Variants { get; init; }

        public async ValueTask DisposeAsync()
        {
            foreach (var variant in Variants)
                await variant.Content.DisposeAsync();

            await Original.DisposeAsync();
        }
    }
}
