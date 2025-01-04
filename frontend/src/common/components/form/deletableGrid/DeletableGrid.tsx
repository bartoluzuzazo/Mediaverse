import { Dispatch, SetStateAction } from 'react'

type Props<T extends any> = {
  label: string
  collection: T[]
  setCollection: Dispatch<SetStateAction<T[]>>
  displayFn?: Function
}

export const DeletableGrid = <T extends any> ({label, collection, setCollection, displayFn} : Props<T>) => {

  const handleDelete = (data: T) => {
    setCollection(collection.filter((_item, i) => i!==collection.indexOf(data)));
  }

  return (
    <>
      <label className="p-1">{label}</label>
      <div className="grid grid-cols-1 divide-y divide-black outline outline-2 outline-slate-500 rounded-md">
        {collection?.length > 0 ?
          collection.map(item =>
            (<div className="flex flex-row justify-between">
              <div className="p-1" key={displayFn ? displayFn(item) : item}>{displayFn ? displayFn(item) : item}</div>
              <div className="p-1 border-l-2 border-slate-500" onClick={() => handleDelete(item)}>Delete</div>
            </div>)) :
          <div className="p-1">None</div>}
      </div>
    </>
  )
}