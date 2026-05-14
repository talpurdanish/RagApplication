export interface GenreModel {
  id: number;
  name: string;
}

export function createGenreModel(raw: any): GenreModel {
  return {
    id: raw.id ?? 0,
    name: raw.name ?? '',
  };
}

export interface ActorModel {
  id: number;
  name: string;
}

export function createActorModel(raw: any): ActorModel {
  return {
    id: raw.id ?? 0,
    name: raw.name ?? '',
  };
}

export interface KeywordModel {
  id: number;
  name: string;
}

export function createKeywordModel(raw: any): KeywordModel {
  return {
    id: raw.id ?? 0,
    name: raw.name ?? '',
  };
}

export interface ProductionCompanyModel {
  id: number;
  name: string;
}

export function createProductionCompanyModel(raw: any): ProductionCompanyModel {
  return {
    id: raw.id ?? 0,
    name: raw.name ?? '',
  };
}

export interface VoteModel {
  id: number;
  movieId: number;
  like: boolean;
}

export function createVoteModel(raw: any): VoteModel {
  return {
    id: raw.id ?? 0,
    movieId: raw.movieId ?? 0,
    like: raw.like ?? false,
  };
}

export interface ProductionCountryModel {
  id: number;
  name: string;
}

export function createProductionCountryModel(raw: any): ProductionCountryModel {
  return {
    id: raw.id ?? 0,
    name: raw.name ?? '',
  };
}

export interface SpokenLanguageModel {
  id: number;
  name: string;
}

export function createSpokenLanguageModel(raw: any): SpokenLanguageModel {
  return {
    id: raw.id ?? 0,
    name: raw.name ?? '',
  };
}

export interface MovieModel {
  id: number;
  budget: number;
  homepage: string;
  language: string;
  title: string;
  overview: string;
  popularity: number;
  releaseDate: string;
  revenue: number;
  runtime: number;
  status: string;
  tagline: string;
  voteAverage: number;
  voteCount: number;
  directorId: number;
  director: string;
  similarity: number;
  actors: ActorModel[];
  genres: GenreModel[];
  keywords: KeywordModel[];
  companies: ProductionCompanyModel[];
  countries: ProductionCountryModel[];
  spokenLanguages: SpokenLanguageModel[];

  actorsString: string;
  genresString: string;
  keywordsString: string;
  companiesString: string;
  countriesString: string;
  spokenLanguagesString: string;
}

export function createMovieModel(raw: any): MovieModel | null {
  try {
    const actors = (raw.actors ?? []).map((a: any) => createActorModel(a));
    const genres = (raw.genres ?? []).map((a: any) => createGenreModel(a));
    const keywords = (raw.keywords ?? []).map((a: any) => createKeywordModel(a));
    const companies = (raw.companies ?? []).map((a: any) => createProductionCompanyModel(a));
    const countries = (raw.countries ?? []).map((a: any) => createProductionCountryModel(a));
    const spokenLanguages = (raw.spokenLanguages ?? []).map((a: any) =>
      createSpokenLanguageModel(a),
    );

    return {
      id: raw.id ?? 0,
      budget: raw.budget ?? 0,
      homepage: raw.homepage ?? '',
      language: raw.language ?? '',
      title: raw.title ?? '',
      overview: raw.overview ?? '',
      popularity: raw.popularity ?? 0,
      releaseDate: raw.releaseDate ?? '',
      revenue: raw.revenue ?? 0,
      runtime: raw.runtime ?? 0,
      status: raw.status ?? '',
      tagline: raw.tagline ?? '',
      voteAverage: raw.voteAverage ?? 0,
      voteCount: raw.voteCount ?? 0,
      directorId: raw.directorId ?? 0,
      director: raw.director ?? '',
      similarity: raw.similarity ?? '',

      actors: actors,
      genres: genres,
      keywords: keywords,
      companies: companies,
      countries: countries,
      spokenLanguages: spokenLanguages,

      actorsString: actors.map((a: any) => a.name).join(', '),
      genresString: genres.map((a: any) => a.name).join(', '),
      keywordsString: keywords.map((a: any) => a.name).join(', '),
      companiesString: companies.map((a: any) => a.name).join(', '),
      countriesString: countries.map((a: any) => a.name).join(', '),
      spokenLanguagesString: spokenLanguages.map((a: any) => a.name).join(', '),
    };
  } catch (e) {
    console.log(e);
    return null;
  }
}

export interface PagedResults {
  movies: MovieModel[];
  currentPage: number;
  totalRecords: number;
  totalPages: number;
  pageSize: number;
  nextPage?: number;
  prevPage?: number;
}

export function createPagedResults(raw: any): PagedResults {
  return {
    movies: (raw.movieViewModels ?? []).map((m: any) => createMovieModel(m)),
    currentPage: raw.currentPage ?? 0,
    totalRecords: raw.totalRecords ?? 0,
    totalPages: raw.totalPages ?? 0,
    pageSize: raw.pageSize ?? 0,
    nextPage: raw.nextPage ?? -1,
    prevPage: raw.prevPage ?? -1,
  };
}

export function createDefaultPagedResults(): PagedResults {
  return {
    movies: [],
    currentPage: -1,
    totalRecords: -1,
    totalPages: -1,
    pageSize: -1,
    nextPage: -1,
    prevPage: -1,
  };
}

export interface Weights {
  budget: number;
  homepage: number;
  title: number;
  overview: number;
  popularity: number;
  status: number;
  tagline: number;
  voteAverage: number;
  voteCount: number;
  directorName: number;
  genres: number;
  casts: number;
  keywords: number;
  productionCompanies: number;
  productionCountries: number;
  spokenLanguages: number;
  query?: string;
  type?: number;
}

export function convertWeights(weights: Weights): Record<string, string> {
  if (!weights) return {};
  return {
    budget: String(weights.budget),
    homepage: String(weights.homepage),
    title: String(weights.title),
    overview: String(weights.overview),
    popularity: String(weights.popularity),
    status: String(weights.status),
    tagline: String(weights.tagline),
    voteAverage: String(weights.voteAverage),
    voteCount: String(weights.voteCount),
    directorName: String(weights.directorName),
    genres: String(weights.genres),
    casts: String(weights.casts),
    keywords: String(weights.keywords),
    productionCompanies: String(weights.productionCompanies),
    productionCountries: String(weights.productionCountries),
    spokenLanguages: String(weights.spokenLanguages),
    query: weights.query ?? '',
  };
}
